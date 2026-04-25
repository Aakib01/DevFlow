using DevFlow.Identity.Entities;
using DevFlow.Shared.Kernel.Results;

namespace DevFlow.Identity.Processor
{
    public interface IIdentityProcessor
    {
        public Task<Result<string>> LoginAsync(string userName, string password);

        public Task<Result<bool>> RegisterUserAsync(string userName, string email, string password, int tenantId);
    
        public Task<Result<bool>> RegisterTenantAsync(string tenantName);

        public Task<bool> IsAdmin(int userId, int workspaceId);

        public Task<bool> AddWorkspaceMember(int userId, int workspaceId, string role);

        public Task<bool> CreateWorkspace(string name, int tenantId, int userId);
        public Task<WorkspaceMember> GetMemberById(int memberId);
    }
}
