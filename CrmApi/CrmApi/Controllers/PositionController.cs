using System.Linq;
using System.Threading.Tasks;
using CrmApi.Data;
using CrmApi.DataTransferObjects;
using CrmApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CrmApi.Controllers
{
    [Route("api/positions")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        private readonly ApplicationContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<PositionController> _logger;
        public PositionController(ApplicationContext dbContext, UserManager<User> userManager, ILogger<PositionController> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        [Route("{category}")]
        [Authorize]
        public async Task<IActionResult> GetPositionByCategoryId(int categoryId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity?.Name);

            var positions = _dbContext.Positions
                .Where(p => p.CategoryId == categoryId && p.User.Id == user.Id);

            return Ok(positions);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePosition(string name, double cost, int categoryId)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
            {
                _logger.LogError($"{nameof(CreatePosition)}: Creating new position failed because no category" + 
                                 " with provided id was found in database.");
                return BadRequest();
            }

            var user = await _userManager.FindByNameAsync(User.Identity?.Name);

            await _dbContext.Positions.AddAsync(new Position() { Name = name, Cost = cost, Category = category, User = user });
            await _dbContext.SaveChangesAsync();

            return StatusCode(201);
        }

        [Route("{id:int}")]
        [HttpPut]
        public async Task<IActionResult> UpdatePosition(int id, [FromBody] PositionModifyDto positionDto)
        {
            var position = await _dbContext.Positions.FirstOrDefaultAsync(p => p.Id == id);

            if (position == null)
            {
                _logger.LogWarning($"{nameof(UpdatePosition)}: no position with id {id} exists.");
                return BadRequest();
            }

            position.Name = positionDto.Name;
            position.Cost = positionDto.Cost;
            position.CategoryId = positionDto.CategoryId;

            await _dbContext.SaveChangesAsync();

            return Ok(nameof(UpdatePosition));
        }

        [Route("{id:int}")]
        [HttpDelete]
        public async Task<IActionResult> DeletePosition(int id)
        {
            var position = await _dbContext.Positions.FirstOrDefaultAsync(p => p.Id == id);

            if (position == null)
            {
                var errorMessage = $"No position with id {id} exists";
                _logger.LogWarning($"{nameof(DeletePosition)}: {errorMessage}");
                return BadRequest(new {message = errorMessage});
            }

            _dbContext.Positions.Remove(position);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}