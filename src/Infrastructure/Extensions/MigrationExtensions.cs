using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

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
