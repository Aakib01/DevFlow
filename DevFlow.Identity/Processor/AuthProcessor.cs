using DevFlow.Identity.Entities;
using DevFlow.Identity.Repository;
using DevFlow.Identity.Services;
using DevFlow.Shared.Kernel.Results;

namespace DevFlow.Identity.Processor
{
    public class AuthProcessor : IAuthProcessor
    {
        AuthRepository _authRepository;
        JwtService _jwt;

        public AuthProcessor(AuthRepository authRepository, JwtService jwt) 
        { 
             _authRepository = authRepository;
             _jwt = jwt;
        }
        

        public async Task<Result<string>> LoginAsync(string userName, string password)
        {
            User user = new User
            {
                UserName = userName,
                PasswordHash = password
            };
            var userDetails = await _authRepository.Login(user);

            var token = _jwt.GenerateToken(user);

            if (user == null)
            {
                return Result<string>.Failure("Invalid email or password.");
            }

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
    }
}
