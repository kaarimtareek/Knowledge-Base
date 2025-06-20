using System.Linq.Expressions;
using KnowledgeBase.Core.Interfaces;
using KnowledgeBase.Core.Models;
using KnowledgeBase.Core.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBase.Infrastructure;

public class KnowledgeBaseDbContext : DbContext, IApplicationDbContext
{
    public KnowledgeBaseDbContext(DbContextOptions<KnowledgeBaseDbContext> options)
        : base(options)
    {
    }

    public DbSet<Topic> Topics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        ApplyGlobalFilters(modelBuilder);
    }

    

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // You can add custom logic here before saving changes, like auditing or validation
        BeforeSaveChanges();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        // You can add custom logic here before saving changes, like auditing or validation
        BeforeSaveChanges();
        return base.SaveChanges();
    }
    //apply query filters to filter out soft-deleted entities


    #region Helpers

    private static void ApplyGlobalFilters(ModelBuilder modelBuilder)
    {
        // Example of applying a global filter for soft-deleted entities
        var softDeletableEntityTypes = modelBuilder.Model.GetEntityTypes()
            .Where(et => typeof(ISoftDeletable).IsAssignableFrom(et.ClrType));
        // Apply the filter to each of them
        foreach (var entityType in softDeletableEntityTypes.Select(x=> x.ClrType))
        {
            modelBuilder.Entity(entityType)
                .HasQueryFilter(ConvertToDeleteFilter(entityType));
        }
    }

// It's cleaner to extract the lambda creation to a helper method
    private static LambdaExpression ConvertToDeleteFilter(Type type)
    {
        // e => !EF.Property<bool>(e, "IsDeleted")
        var parameter = Expression.Parameter(type, "e");
        var property = Expression.Call(typeof(EF), nameof(EF.Property), new[] { typeof(bool) }, parameter,
            Expression.Constant("IsDeleted"));
        var not = Expression.Not(property);
        var lambda = Expression.Lambda(not, parameter);
        return lambda;
    }

    private void BeforeSaveChanges()
    {
        // Custom logic before saving changes, e.g., setting timestamps or auditing
        foreach (var entry in ChangeTracker.Entries())
        {
            if ((entry.State == EntityState.Added || entry.State == EntityState.Modified) && entry.Entity is BaseEntity baseEntity)
            {
                // Set timestamps
                if (entry.State == EntityState.Added)
                {
                    baseEntity.Id = Ulid.NewUlid().ToGuid(); // Ensure Id is set for new entities
                    baseEntity.CreatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    baseEntity.UpdatedAt = DateTime.UtcNow;
                }
            }
        }
    }

    #endregion
}