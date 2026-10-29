using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using BBDbContext dbContext = app.ApplicationServices
            .GetRequiredService<IDbContextFactory<BBDbContext>>()
            .CreateDbContext();

        dbContext.Database.Migrate();
    }
}
