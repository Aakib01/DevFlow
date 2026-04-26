using DevFlow.Projects.Entities;
using DevFlow.Projects.Infrastructure.Data;
using DevFlow.Shared.Kernel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Projects.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ITenantContext _tenant;

        public TicketController(AppDbContext db, ITenantContext tenantContext)
        {
            _db = db;
            _tenant = tenantContext;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            ticket.TenantId = _tenant.TenantId;
            _db.Tickets.Add(ticket);
            await _db.SaveChangesAsync();

            return Ok(ticket);
        }

        [HttpGet]
        public async Task<IActionResult> Get(int projectId, string? state, int page = 1, int pageSize = 10)
        {
            var query = _db.Tickets.Where(t => t.ProjectId == projectId && t.TenantId == _tenant.TenantId);

            if (!string.IsNullOrEmpty(state))
                query = query.Where(t => t.State == state);

            var result = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(result);
        }
    }
}
