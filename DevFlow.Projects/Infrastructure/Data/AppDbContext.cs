using DevFlow.Projects.Entities;
using DevFlow.Shared.Kernel.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using DevFlow.Shared.Kernel.Interfaces;

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
        public DbSet<WorkflowState> WorkflowStates => Set<WorkflowState>();
        public DbSet<WorkflowTransition> WorkflowTransitions => Set<WorkflowTransition>();

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
            var tenantIdProperty = Expression.Property(
                Expression.Constant(_tenantContext),
                typeof(ITenantContext).GetProperty(nameof(ITenantContext.TenantId))!);

            var body = Expression.Equal(property, tenantIdProperty);
            return Expression.Lambda(body, parameter);
        }
    }
}
