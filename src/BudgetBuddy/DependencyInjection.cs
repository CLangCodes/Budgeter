using System.Reflection;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;

namespace BudgetBuddy;

public static class DependencyInjection
{
    public static IServiceCollection AddServer(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddOpenApiWithAuth();
        services.AddEndpoints(Assembly.GetExecutingAssembly());

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}