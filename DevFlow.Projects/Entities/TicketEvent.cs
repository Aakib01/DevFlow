namespace DevFlow.Projects.Entities
{
    public class TicketEvent
    {
        public int Id { get; set; }

        public int TenantId { get; set; }
        public int TicketId { get; set; }

        public string EventType { get; set; } = string.Empty;

        public string Payload { get; set; } = string.Empty;

        public int ActorId { get; set; }

        public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    }
}
