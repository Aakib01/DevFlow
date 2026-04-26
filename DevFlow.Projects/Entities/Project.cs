using DevFlow.Shared.Kernel.Entities;
using DevFlow.Shared.Kernel.Entities;

namespace DevFlow.Projects.Entities
{
    public class Project : TenantedEntity
    {
        public string Name { get; set; } = string.Empty;
    }
}
