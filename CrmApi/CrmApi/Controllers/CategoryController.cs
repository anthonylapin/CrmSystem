using Microsoft.AspNetCore.Mvc;

namespace CrmApi.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCategories()
        {
            return Ok(nameof(GetCategories));
        }

        [Route("{id:int}")]
        [HttpGet]
        public IActionResult GetCategory(int id)
        {
            return Ok(nameof(GetCategory));
        }

        [HttpPost]
        public IActionResult CreateCategory()
        {
            return Ok(nameof(CreateCategory));
        }

        [Route("{id:int}")]
        [HttpDelete]
        public IActionResult DeleteCategory(int id)
        {
            return Ok(nameof(DeleteCategory));
        }

        [Route("{id:int}")]
        [HttpPatch]
        public IActionResult UpdateCategory(int id)
        {
            return Ok(nameof(UpdateCategory));
        }
    }
}