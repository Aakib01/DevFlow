using DevFlow.Projects.Entities;
using DevFlow.Projects.Infrastructure.Data;
using DevFlow.Projects.Services;
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
        private readonly WorkflowService _workflowService;

        public TicketController(AppDbContext db, ITenantContext tenantContext, WorkflowService workflowService)
        {
            _db = db;
            _tenant = tenantContext;
            _workflowService = workflowService;
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

        [HttpPost("{id}/transition")]
        public async Task<IActionResult> Transition(int id, int toStateId)
        {
            var role = User.FindFirst("role")?.Value ?? "Member";

            var success = await _workflowService.Transition(id, toStateId, role);

            if (!success)
                return BadRequest("Invalid transition");

            return Ok();
        }

        [HttpGet("{id}/transitions")]
        public async Task<IActionResult> GetTransitions(int id)
        {
            var role = User.FindFirst("role")?.Value ?? "Member";

            var transitions = await _workflowService
                .GetAvailableTransitions(id, role);

            return Ok(transitions);
        }
    }
}
