using DevFlow.Identity.Entities;
using DevFlow.Shared.Kernel.Results;

namespace DevFlow.Identity.Processor
{
    public interface IAuthProcessor
    {
        public Task<Result<string>> LoginAsync(string userName, string password);

        public Task<Result<bool>> RegisterUserAsync(string userName, string email, string password, int tenantId);
    
        public Task<Result<bool>> RegisterTenantAsync(string tenantName);
      

    }
}
