using BudgetBuddy.Infrastructure;

namespace BudgetBuddy;

public static class DependencyInjection
{
    public static IServiceCollection AddServer(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}