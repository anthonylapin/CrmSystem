using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CrmApi.Data;
using CrmApi.Extensions;
using CrmApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _appEnvironment;

        public CategoryController(ApplicationContext dbContext,
            UserManager<User> userManager, ILogger<PositionController> logger, IWebHostEnvironment appEnvironment)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
            _appEnvironment = appEnvironment;
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
        [DisableRequestSizeLimit]
        public async Task<IActionResult> CreateCategory()
        {
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.First();

            if (file.Length == 0 || !formCollection.ContainsKey("name") || !file.IsImage())
            {
                return BadRequest("Invalid form data.");
            }

            var fileName = file.GetFileName();

            var dbPath = Path.Combine("Files", $"{Guid.NewGuid()}{Path.GetExtension(fileName)}");
            var fullPath = Path.Combine(_appEnvironment.WebRootPath, dbPath);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var categoryName = formCollection["name"].FirstOrDefault();
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);

            var category = new Category() {Name = categoryName, ImageSource = dbPath, UserId = user.Id};
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();

            return Ok(_dbContext.Categories.ToArray());
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