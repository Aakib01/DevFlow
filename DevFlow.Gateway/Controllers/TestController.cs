using DevFlow.Shared.Kernel.Interfaces;
using DevFlow.Gateway.Services;
using DevFlow.Shared.Kernel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevFlow.Gateway.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet("test")]
        public IActionResult Test(ITenantContext tenantContext)
        {
            return Ok(new
            {
                IsAuthenticated = User.Identity?.IsAuthenticated,
                UserId = User.FindFirst("userId")?.Value,
                TenantId = tenantContext.TenantId
            });
        }
    }
}
