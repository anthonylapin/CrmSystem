using System.Linq;
using CrmApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CrmApi.Models;
using CrmApi.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CrmApi.Controllers
{
    [Route("api/analytics")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        private readonly ApplicationContext _dbContext;
        private readonly UserManager<User> _userManager;

        public AnalyticsController(ApplicationContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        
        [Route("overview")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAnalyticsOverview()
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);

            var orders = await _dbContext.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == user.Id)
                .OrderBy(o => o.Date)
                .ToListAsync();

            var result = new AnalyticsBuilder()
                .FromOrders(orders)
                .Build();
            
            return Ok(result);
        }
    }
}