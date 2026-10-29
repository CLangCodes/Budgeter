using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddDatabase(configuration);

    private static IServiceCollection AddDatabase(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContextFactory<BBDbContext>(options =>
            options.UseSqlite("Data Source=budget-buddy.db;"));

        return services;
    }
}