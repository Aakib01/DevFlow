using DevFlow.Projects.Entities;
using DevFlow.Projects.Infrastructure.Data;

namespace DevFlow.Projects.Services
{
    public class EventService
    {
        private readonly AppDbContext _db;

        public EventService(AppDbContext db)
        {
            _db = db;
        }

        public async Task LogEvent(int tenantId, int ticketId, string eventType, object payload, int actorId)
        {
            var ev = new TicketEvent
            {
                TenantId = tenantId,
                TicketId = ticketId,
                EventType = eventType,
                Payload = System.Text.Json.JsonSerializer.Serialize(payload),
                ActorId = actorId
            };

            _db.TicketEvents.Add(ev);
            await _db.SaveChangesAsync();
        }
    }
}
