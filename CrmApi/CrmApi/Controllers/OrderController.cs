using Microsoft.AspNetCore.Mvc;

namespace CrmApi.Controllers
{
    [Route("/api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetOrder()
        {
            return Ok("Get Order");
        }

        [HttpPost]
        public IActionResult CreateOrder()
        {
            return Ok("Create order");
        }
    }
}