using Microsoft.AspNetCore.Mvc;

namespace CrmApi.Controllers
{
    [Route("api/analytics")]
    [ApiController]
    public class AnalyticsController : ControllerBase
    {
        [Route("overview")]
        [HttpGet]
        public IActionResult GetAnalyticsOverview()
        {
            return Ok(nameof(GetAnalyticsOverview));
        }

        [Route("analytics")]
        [HttpGet]
        public IActionResult GetAnalytics()
        {
            return Ok(nameof(GetAnalytics));
        }
    }
}