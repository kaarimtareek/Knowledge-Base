using KnowledgeBase.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace KnowledgeBase.API.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrations(this IApplicationBuilder app)
    {
        // We need a service scope to resolve our services, including the DbContext.
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        // Get the DbContext from the dependency injection container.
        using KnowledgeBaseDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<KnowledgeBaseDbContext>();

        // Get a logger to report the result.
        ILogger<KnowledgeBaseDbContext> logger =
            scope.ServiceProvider.GetRequiredService<ILogger<KnowledgeBaseDbContext>>();
        logger.LogInformation("Applying database migrations...");

        // This is the command that runs pending migrations.
        await dbContext.Database.MigrateAsync();

        logger.LogInformation("Database migrations applied successfully.");
    }
}