using Microsoft.AspNetCore.Mvc;
using DevFlow.Projects.Entities;
using DevFlow.Projects.Infrastructure.Data;

namespace DevFlow.Projects.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ProjectController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            _db.Projects.Add(project);
            await _db.SaveChangesAsync();

            return Ok(project);
        }
    }
}
