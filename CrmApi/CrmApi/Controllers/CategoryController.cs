using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CrmApi.Data;
using CrmApi.Extensions;
using CrmApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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

        [Authorize]
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> CreateCategory()
        {
            var (categoryName, image) = await ReadCategoryFormData();

            if (categoryName == null)
            {
                return BadRequest(new ErrorDetails() {Message = "Category Name is required."});
            }
            
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);
            
            var category = new Category() {Name = categoryName, UserId = user.Id};

            if (image != null)
            {
                if (!image.ValidateImageFile(out var errMessage))
                {
                    return BadRequest(new ErrorDetails() { Message = errMessage });
                }

                category.ImageSource = await SaveCategoryImage(image);
            }
            
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync(); 

            return Ok(category);
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

            if (category.ImageSource != null)
            {
                DeleteCategoryImage(category.ImageSource);
            }
            
            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [Route("{id:int}")]
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var category = _dbContext.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound(new ErrorDetails() { Message = "No such category exists" });
            }
            
            var (categoryName, image) = await ReadCategoryFormData();
            
            if (categoryName == null)
            {
                return BadRequest(new ErrorDetails() {Message = "Category Name is required."});
            }
            
            if (image != null)
            {
                if (!image.ValidateImageFile(out var errMessage))
                {
                    return BadRequest(new ErrorDetails() { Message = errMessage });
                }

                if (category.ImageSource != null)
                {
                    DeleteCategoryImage(category.ImageSource);
                }
    
                category.ImageSource = await SaveCategoryImage(image);
            }

            await _dbContext.SaveChangesAsync();
            
            return Ok(category);
        }

        private string GetCategoryImageFullPath(string imagePath) =>
            Path.Combine(_appEnvironment.WebRootPath, imagePath);

        private async Task<string> SaveCategoryImage(IFormFile file)
        {
            var fileName = file.GetFileName();
            var dbPath = Path.Combine("Files", $"{Guid.NewGuid()}{Path.GetExtension(fileName)}");
            var fullPath = GetCategoryImageFullPath(dbPath);

            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            return dbPath;
        }

        private void DeleteCategoryImage(string imagePath)
        {
            var fullImagePath = GetCategoryImageFullPath(imagePath);
            if (System.IO.File.Exists(fullImagePath))
            {
                System.IO.File.Delete(fullImagePath);
                _logger.LogInformation($"File was deleted {fullImagePath}");
            }
        }

        private async Task<CategoryModifyDto> ReadCategoryFormData()
        {
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.FirstOrDefault();
            var categoryName = formCollection["name"].FirstOrDefault();
            return new CategoryModifyDto() { Name = categoryName, Image = file };
        }
    }
}