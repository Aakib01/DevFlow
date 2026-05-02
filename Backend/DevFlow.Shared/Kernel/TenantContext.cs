using DevFlow.Shared.Kernel.Interfaces;

namespace DevFlow.Shared.Kernel
{
    public class TenantContext : ITenantContext
    {
        public int TenantId { get; set; }
    }
}