using DevFlow.Projects.Entities;
using DevFlow.Projects.Infrastructure.Data;
using DevFlow.Projects.Services;
using DevFlow.Shared.Kernel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Net.Sockets;

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
        private readonly EventService _eventService;
        private readonly CacheService _cache;
        private readonly EventPublisher _eventPublisher;

        public TicketController(AppDbContext db, ITenantContext tenantContext, WorkflowService workflowService,
            EventService eventService, CacheService cacheService, EventPublisher eventPublisher)
        {
            _db = db;
            _tenant = tenantContext;
            _workflowService = workflowService;
            _eventService = eventService;
            _cache = cacheService;
            _eventPublisher = eventPublisher;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            ticket.TenantId = _tenant.TenantId;
            _db.Tickets.Add(ticket);
            await _db.SaveChangesAsync();

            await _eventService.LogEvent(_tenant.TenantId,ticket.Id,"TicketCreated",
                new { ticket.Title },int.Parse(User.FindFirst("sub")?.Value ?? "0"));
        

            return Ok(ticket);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var cacheKey = $"tenant:{_tenant.TenantId}:ticket:{id}";

            var cached = await _cache.GetAsync<Ticket>(cacheKey);

            if (cached != null)
                return Ok(cached);

            var ticket = await _db.Tickets
                .FirstOrDefaultAsync(t => t.Id == id && t.TenantId == _tenant.TenantId);

            if (ticket == null)
                return NotFound();

            await _cache.SetAsync(cacheKey, ticket);

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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Ticket updatedTicket)
        {
            var cacheKey = $"tenant:{_tenant.TenantId}:ticket:{id}";

            var ticket = await _db.Tickets
                .FirstOrDefaultAsync(t => t.Id == id && t.TenantId == _tenant.TenantId);

            if (ticket == null)
                return NotFound();

            await _eventPublisher.PublishAsync("ticket-events", new
            {
                TicketId = ticket.Id,
                Event = "StatusChanged",
                From = ticket,
                To = updatedTicket
            });


            // Update fields
            ticket.ProjectId = updatedTicket.ProjectId;
            ticket.Title = updatedTicket.Title;
            ticket.AssignedUserId = updatedTicket.AssignedUserId;
            ticket.State = updatedTicket.State;

            await _db.SaveChangesAsync();

            await _eventService.LogEvent(
                _tenant.TenantId,
                ticket.Id,
                "TicketUpdated",
                new { ticket.Title },
                int.Parse(User.FindFirst("sub")?.Value ?? "0")
            );

            
            await _cache.SetAsync(cacheKey, ticket);

            return Ok(ticket);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cacheKey = $"tenant:{_tenant.TenantId}:ticket:{id}";

            var ticket = await _db.Tickets
                .FirstOrDefaultAsync(t => t.Id == id && t.TenantId == _tenant.TenantId);

            if (ticket == null)
                return NotFound();

            _db.Tickets.Remove(ticket);
            await _db.SaveChangesAsync();

            await _eventService.LogEvent(
                _tenant.TenantId,
                id,
                "TicketDeleted",
                null,
                int.Parse(User.FindFirst("sub")?.Value ?? "0")
            );

            await _cache.DeleteAsync(cacheKey);

            return NoContent();
        }

        [HttpPost("{id}/transition")]
        public async Task<IActionResult> Transition(int id, int toStateId)
        {
            var role = User.FindFirst("role")?.Value ?? "Member";

            var result = await _workflowService.Transition(id, toStateId, role);

            if (!result.success)
                return BadRequest("Invalid transition");

            await _eventService.LogEvent(_tenant.TenantId, id,"StatusChanged",
                new { From = result.fromState, To = result.toState }, int.Parse(User.FindFirst("sub")?.Value ?? "0"));

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

        [HttpGet("{id}/activity")]
        public async Task<IActionResult> GetActivity(int id)
        {
            var events = await _db.TicketEvents
                .Where(e => e.TicketId == id)
                .OrderBy(e => e.OccurredAt)
                .ToListAsync();

            return Ok(events);
        }
    }
}
