using DevFlow.Shared.Kernel.Interfaces;

namespace DevFlow.Gateway.Services
{
    public class TenantContext : ITenantContext
    {
        public int TenantId { get; set; }

    }
}
