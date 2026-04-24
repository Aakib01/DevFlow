using DevFlow.Identity.Entities;
using DevFlow.Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Identity.Repository
{
    public class AuthRepository
    {
        IdentityDbContext _db;
        public AuthRepository(IdentityDbContext db) 
        {
            _db = db;
        }

        public async Task<bool> RegisterTenant(Tenant tenant)
        {
            _db.Tenants.Add(tenant);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RegisterUser(User user)
        {          
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<User?> Login(User user)
        {
            var userDetails = await _db.Users
                .FirstOrDefaultAsync(x => x.UserName == user.UserName && x.PasswordHash == user.PasswordHash);
            return userDetails;
        }

        public async Task<int> GetNextTenantId()
        {
            var tenant = await _db.Tenants
                .OrderByDescending(x => x.Id)
                .FirstOrDefaultAsync();
            return tenant != null ? tenant.Id + 1 : 1;
        }
    }
}
