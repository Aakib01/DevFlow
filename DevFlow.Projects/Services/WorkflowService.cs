using DevFlow.Projects.Entities;
using DevFlow.Projects.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Projects.Services
{
    public class WorkflowService
    {
        private readonly AppDbContext _db;

        public WorkflowService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<WorkflowState>> GetAvailableTransitions(int ticketId, string userRole)
        {
            var ticket = await _db.Tickets.FindAsync(ticketId);

            var currentState = await _db.WorkflowStates
                .FirstAsync(x => x.Name == ticket.State);

            var transitions = _db.WorkflowTransitions
                .Where(t => t.FromStateId == currentState.Id);

            if (userRole != "Admin")
            {
                transitions = transitions.Where(t =>
                    t.RequiredRole == null || t.RequiredRole == userRole);
            }

            var nextStates = await _db.WorkflowStates
                .Where(s => transitions.Select(t => t.ToStateId).Contains(s.Id))
                .ToListAsync();

            return nextStates;
        }

        public async Task<(bool success, string? fromState, string? toState)> Transition(int ticketId, int toStateId, string userRole)
        {
            var ticket = await _db.Tickets.FindAsync(ticketId);

            var currentState = await _db.WorkflowStates
                .FirstAsync(x => x.Name == ticket.State);

            var newState = await _db.WorkflowStates.FindAsync(toStateId);

            var valid = await _db.WorkflowTransitions.AnyAsync(t =>
                t.FromStateId == currentState.Id &&
                t.ToStateId == toStateId &&
                (t.RequiredRole == null || t.RequiredRole == userRole)
            );

            if (!valid)
                return (false, null, null);

            var oldStateName = currentState.Name;
            var newStateName = newState.Name;

            ticket.State = newStateName;

            await _db.SaveChangesAsync();

            return (true, oldStateName, newStateName);
        }

    }
}
