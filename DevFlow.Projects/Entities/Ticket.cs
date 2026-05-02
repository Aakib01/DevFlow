using DevFlow.Shared.Kernel.Entities;

namespace DevFlow.Projects.Entities
{
    public class Ticket : TenantedEntity
    {
        public int ProjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string State { get; set; } = "Backlog";
        public int AssignedUserId { get; set; }
    }
}
