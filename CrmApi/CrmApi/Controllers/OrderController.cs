using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        [Route("count")]
        public IActionResult GetOrdersCount([FromQuery] OrderFilterQueryDto orderFilterQueryDto)
        {
            var count = _dbContext.Orders
                .ToList()
                .Count(o => GetOrderFilterResult(o, orderFilterQueryDto));
            return Ok(count);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllOrders([FromQuery] OrderQueryDto orderQueryDto)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);

            var orders = _dbContext.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == user.Id)
                .OrderByDescending(o => o.Date)
                .Skip(orderQueryDto.Offset)
                .Take(orderQueryDto.Limit)
                .ToList()
                .Where(o => GetOrderFilterResult(o, orderQueryDto));

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

            var orderItems = orderDto.OrderItems
                .Select(oi => new OrderItem {Name = oi.Name, Quantity = oi.Quantity, Cost = oi.Cost})
                .ToList();

            var order = new Order()
            {
                OrderItems = orderItems,
                OrderNumber = maxOrder + 1,
                UserId = user.Id
            };

            await _dbContext.Orders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return StatusCode(201, order);
        }
        
        private bool GetOrderFilterResult(Order order, OrderFilterQueryDto orderFilterQueryDto)
        {
            var orderQueryPredicates = new List<Func<Order, bool>>();

            if (orderFilterQueryDto.Start != null)
            {
                orderQueryPredicates.Add(o => o.Date >= orderFilterQueryDto.Start);
            }

            if (orderFilterQueryDto.End != null)
            {
                orderQueryPredicates.Add(o => o.Date <= orderFilterQueryDto.End?.AddDays(1));
            }

            if (orderFilterQueryDto.Order != null)
            {
                orderQueryPredicates.Add(o => o.OrderNumber == orderFilterQueryDto.Order);
            }

            return orderQueryPredicates.Aggregate(true, (result, next) => result && next(order));
        }
    }
}