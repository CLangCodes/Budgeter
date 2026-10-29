using Infrastructure.Data;
using Infrastructure.Time;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddServices()
            .AddDatabase(configuration);

    private static IServiceCollection AddDatabase(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContextFactory<BBDbContext>(options =>
            options.UseSqlite("Data Source=budget-buddy.db;"));

        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}