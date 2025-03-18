using Microsoft.AspNetCore.Mvc;

namespace CashFlowly.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("OK.");
        }
    }
}