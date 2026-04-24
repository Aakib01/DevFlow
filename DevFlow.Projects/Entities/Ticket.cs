using DevFlow.Shared.Kernel;

namespace DevFlow.Projects.Entities
{
    public class Ticket : TenantedEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string CurrentState { get; set; } = "Backlog";
    }
}
