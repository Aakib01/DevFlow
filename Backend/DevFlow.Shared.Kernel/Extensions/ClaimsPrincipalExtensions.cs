using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DevFlow.Shared.Kernel.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static int GetTenantId(this ClaimsPrincipal user)
        {
            var tenantId = user.FindFirst("tenantId")?.Value;
            return tenantId != null ? int.Parse(tenantId) : 0;
        }
    }
}
