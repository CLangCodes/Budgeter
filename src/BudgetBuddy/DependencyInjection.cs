using System.Reflection;
using System.Text.Json.Serialization;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;

namespace BudgetBuddy;

public static class DependencyInjection
{
    public static IServiceCollection AddServer(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        // Send/receive enums by name (e.g. "Month") rather than their numeric value
        services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        services.AddOpenApiWithAuth();
        services.AddEndpoints(Assembly.GetExecutingAssembly());

        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}