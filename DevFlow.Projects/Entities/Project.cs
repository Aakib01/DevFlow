using DevFlow.Shared.Kernel.Entities;

namespace DevFlow.Projects.Entities
{
    public class Project : TenantedEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
    }
}
