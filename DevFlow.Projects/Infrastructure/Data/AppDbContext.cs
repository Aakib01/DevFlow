using DevFlow.Projects.Entities;
using DevFlow.Shared.Kernel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace DevFlow.Projects.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {   
        private readonly ITenantContext _tenantContext;

        public AppDbContext(DbContextOptions<AppDbContext> options,
                            ITenantContext tenantContext)
            : base(options)
        {
            _tenantContext = tenantContext;
        }

        public DbSet<Project> Projects => Set<Project>();
        public DbSet<Ticket> Tickets => Set<Ticket>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(TenantedEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .HasQueryFilter(GenerateTenantFilter(entityType.ClrType));
                }
            }
        }

        private LambdaExpression GenerateTenantFilter(Type type)
        {
            var parameter = Expression.Parameter(type, "e");
            var property = Expression.Property(parameter, "TenantId");
            var tenantId = Expression.Constant(_tenantContext.TenantId);

            var body = Expression.Equal(property, tenantId);
            return Expression.Lambda(body, parameter);
        }
    }
}
