using DevFlow.Shared.Kernel;

namespace DevFlow.Projects.Infrastructure.Data
{
    public class TenantContext : ITenantContext
    {
        public int TenantId { get; set; } = 1; // Default tenant ID
    }
}
