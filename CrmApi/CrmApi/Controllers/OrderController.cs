using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrmApi.Data;
using CrmApi.DataTransferObjects;
using CrmApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrmApi.Controllers
{
    [Route("/api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationContext _dbContext;
        private readonly UserManager<User> _userManager;

        public OrderController(ApplicationContext dbContext,
            UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllOrders([FromQuery] OrderQueryDto orderQueryDto)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);

            var orderQueryPredicates = new List<Predicate<Order>>() { o => o.UserId == user.Id };

            if (orderQueryDto.Start != null)
            {
                orderQueryPredicates.Add(o => o.Date >= orderQueryDto.Start);
            }

            if (orderQueryDto.End != null)
            {
                orderQueryPredicates.Add(o => o.Date <= orderQueryDto.End);
            }

            if (orderQueryDto.Order != null)
            {
                orderQueryPredicates.Add(o => o.OrderNumber == orderQueryDto.Order);
            }

            var orders = _dbContext.Orders
                .Include(o => o.OrderItems)
                .Where(o => orderQueryPredicates.Aggregate(true ,(result, next) => result && next(o)))
                .Skip(orderQueryDto.Offset)
                .Take(orderQueryDto.Limit)
                .OrderByDescending(o => o.Date);

            return Ok(orders);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);

            var lastOrder = _dbContext.Orders
                .OrderByDescending(o => o.Date)
                .FirstOrDefault(o => o.UserId == user.Id);

            var maxOrder = lastOrder?.OrderNumber ?? 0;

            var order = new Order()
            {
                OrderItems = orderDto.OrderItems,
                OrderNumber = maxOrder + 1,
                UserId = user.Id
            };

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return Ok(order);
        }
    }
}