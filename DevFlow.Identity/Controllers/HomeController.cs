using Microsoft.AspNetCore.Mvc;

namespace DevFlow.Identity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Service is running");
        }
    }
}
