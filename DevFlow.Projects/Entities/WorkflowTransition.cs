using DevFlow.Shared.Kernel.Entities;

namespace DevFlow.Projects.Entities
{
    public class WorkflowTransition : TenantedEntity
    {
        public int Id { get; set; }

        public int ProjectId { get; set; }

        public int FromStateId { get; set; }
        public int ToStateId { get; set; }

        public string? RequiredRole { get; set; }
    }
}
