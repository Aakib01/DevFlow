using DevFlow.Identity.Entities;
using DevFlow.Identity.Repository;
using DevFlow.Identity.Services;
using DevFlow.Shared.Kernel.Results;

namespace DevFlow.Identity.Processor
{
    public class IdentityProcessor : IIdentityProcessor
    {
        AuthRepository _authRepository;
        JwtService _jwt;

        public IdentityProcessor(AuthRepository authRepository, JwtService jwt) 
        { 
             _authRepository = authRepository;
             _jwt = jwt;
        }


        public async Task<Result<string>> LoginAsync(string userName, string password)
        {
            var user = new User
            {
                UserName = userName,
                PasswordHash = password
            };

            var userDetails = await _authRepository.Login(user);

            if (userDetails == null)
            {
                return Result<string>.Failure("Invalid email or password.");
            }

            // ✅ Use the populated user from database, not the empty one
            var token = _jwt.GenerateToken(userDetails);

            return Result<string>.Success(token);
        }

        public async Task<Result<bool>> RegisterUserAsync(string userName, string email, string password, int tenantId)
        {
            User user = new User
            {
                UserName = userName,
                Email = email,
                PasswordHash = password, 
                TenantId = tenantId
            };

            bool isSuccess = await _authRepository.RegisterUser(user);

            if (isSuccess)
            {
                return Result<bool>.Success(true);
            }
            else
            {
                return Result<bool>.Failure("Failed to register user.");
            }
        }

        public async Task<Result<bool>> RegisterTenantAsync(string tenantName)
        {
            Tenant tenant = new Tenant
            {
                Name = tenantName
            };

            bool isSuccess = _authRepository.RegisterTenant(tenant).Result;
            return isSuccess ? Result<bool>.Success(true) : Result<bool>.Failure("Failed to register tenant.");
        }

        public async Task<bool> IsAdmin(int userId, int workspaceId)
        {
            var member = await _authRepository.GetWorkspaceMemeberId(userId, workspaceId);

            return member.Role == "Admin";
        }

        public Task<bool> AddWorkspaceMember(int userId, int workspaceId, string role)
        {
            WorkspaceMember member = new WorkspaceMember
            {
                UserId = userId,
                WorkspaceId = workspaceId,
                Role = role
            };

            return _authRepository.AddWorkspaceMember(member);
        }

        public Task<bool> CreateWorkspace(string name, int tenantId, int userId)
        {
            var workspace = new Workspace
            {
                Name = name,
                TenantId = tenantId
            };

            _authRepository.CreateWorkspace(workspace);

            var admin = new WorkspaceMember
            {
                WorkspaceId = workspace.Id,
                UserId = userId,
                Role = "Admin"
            };

            _authRepository.AddWorkspaceMember(admin);
            return Task.FromResult(true);
        }
    }
}
