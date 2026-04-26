using DevFlow.Projects.Entities;
using DevFlow.Projects.Infrastructure.Data;
using DevFlow.Shared.Kernel.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Projects.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ITenantContext _tenantContext;

        public ProjectController(AppDbContext db, ITenantContext tenantContext )
        {
            _db = db;
            _tenantContext = tenantContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            project.TenantId = _tenantContext.TenantId;
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();

            return Ok(project);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _db.Projects.Where(x => x.TenantId == _tenantContext.TenantId).ToListAsync();
            return Ok(data);
        }
    }
}
