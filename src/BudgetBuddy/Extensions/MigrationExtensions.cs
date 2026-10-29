using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using BBDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<BBDbContext>();

        dbContext.Database.Migrate();
    }
}
