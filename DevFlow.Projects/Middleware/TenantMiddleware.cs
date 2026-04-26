using DevFlow.Shared.Kernel.Interfaces;

namespace DevFlow.Projects.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ITenantContext tenantContext)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var tenantId = context.User.FindFirst("tenantId")?.Value;

                if (tenantId != null)
                {
                    tenantContext.TenantId = int.Parse(tenantId);
                }
            }

            await _next(context);
        }
    }
}