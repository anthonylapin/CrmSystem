using System.Linq;
using System.Threading.Tasks;
using CrmApi.Data;
using CrmApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CrmApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PositionController> _logger;

        public CategoryController(ApplicationContext dbContext, UserManager<User> userManager, ILogger<PositionController> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllCategories()
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            return Ok(_dbContext.Categories.Where(c => c.UserId == user.Id));
        }

        [Route("{id:int}")]
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
            return category == null ? NotFound() : Ok(category);
        }

        [HttpPost]
        public IActionResult CreateCategory()
        {
            return Ok(nameof(CreateCategory));
        }

        [Route("{id:int}")]
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                _logger.LogWarning($"{nameof(DeleteCategory)}: No category with id {id} exists");
                return NotFound();
            }

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [Route("{id:int}")]
        [HttpPatch]
        public IActionResult UpdateCategory(int id)
        {
            return Ok(nameof(UpdateCategory));
        }
    }
}