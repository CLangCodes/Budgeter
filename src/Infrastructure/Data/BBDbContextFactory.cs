using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data;
public sealed class BBDbContextFactory : IDesignTimeDbContextFactory<BBDbContext>
{
    public BBDbContext CreateDbContext(string[] args)
    {
        string? connectionString = "Data Source=budget-buddy.db;";

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "Database connection string is not configured for EF design-time. ");
        }

        var optionsBuilder = new DbContextOptionsBuilder<BBDbContext>();
        optionsBuilder.UseSqlite(connectionString);

        return new BBDbContext(optionsBuilder.Options);
    }
}