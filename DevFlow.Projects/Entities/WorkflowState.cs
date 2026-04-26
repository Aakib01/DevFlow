using DevFlow.Shared.Kernel.Entities;

namespace DevFlow.Projects.Entities
{
    public class WorkflowState : TenantedEntity
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }

        public string Name { get; set; } = string.Empty;
        public int SortOrder { get; set; }

        public bool IsInitial { get; set; }
        public bool IsFinal { get; set; }
    }
}
