using DevFlow.Identity.Entities;
using DevFlow.Identity.Infrastructure.Data;
using DevFlow.Identity.Processor;
using DevFlow.Identity.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Identity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IdentityDbContext _db;
        private readonly JwtService _jwt;
        private readonly IIdentityProcessor _authProcessor;

        public AuthController(IdentityDbContext db, JwtService jwt, IIdentityProcessor authProcessor)
        {
            _db = db;
            _jwt = jwt;
            _authProcessor = authProcessor;
        }

        [HttpPost("registerTenant")]
        public async Task<IActionResult> RegisterTenant(string tenantName)
        {
            var result = await _authProcessor.RegisterTenantAsync(tenantName);

            return Ok(result.IsSuccess);
        }
        [HttpPost("registerUser")]
        public async Task<IActionResult> Register(string userName, string email, string password, int tenantId)
        {          

            var result = await _authProcessor.RegisterUserAsync(userName, email, password, tenantId);

            return Ok(result.IsSuccess);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string userName, string password)
        {   
            var result = await _authProcessor.LoginAsync(userName, password);

            if (!result.IsSuccess)
                return Unauthorized();


            return Ok(result.Value);
        }
    }
}
