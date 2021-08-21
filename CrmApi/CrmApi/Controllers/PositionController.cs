using Microsoft.AspNetCore.Mvc;

namespace CrmApi.Controllers
{
    [Route("api/positions")]
    [ApiController]
    public class PositionController : ControllerBase
    {
        [Route("{category}")]
        [HttpGet]
        public IActionResult GetPositionByCategory(string category)
        {
            return Ok(nameof(GetPositionByCategory));
        }

        [HttpPost]
        public IActionResult CreatePosition()
        {
            return Ok(nameof(CreatePosition));
        }

        [Route("{id:int}")]
        [HttpPatch]
        public IActionResult UpdatePosition(int id)
        {
            return Ok(nameof(UpdatePosition));
        }

        [Route("{id:int}")]
        [HttpDelete]
        public IActionResult DeletePosition(int id)
        {
            return Ok(nameof(DeletePosition));
        }
    }
}