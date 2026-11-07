using System.Reflection;

namespace BudgetBuddy.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddOpenApiWithAuth(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                // InformationalVersion comes from <Version>; .NET appends "+<commit sha>", so strip that
                string? version = typeof(ServiceCollectionExtensions).Assembly
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                    .InformationalVersion
                    .Split('+')[0];

                document.Info.Version = version ?? "0.0.0";
                return Task.CompletedTask;
            });

            // Scalar renders operations in document order; keep DELETE last within each path
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                foreach (var pathItem in document.Paths.Values)
                {
                    if (pathItem.Operations is not { Count: > 1 } operations)
                    {
                        continue;
                    }

                    var ordered = operations
                        .OrderBy(op => op.Key == HttpMethod.Delete)
                        .ToList();

                    operations.Clear();
                    foreach (var (method, operation) in ordered)
                    {
                        operations.Add(method, operation);
                    }
                }

                return Task.CompletedTask;
            });
        });
        return services;
    }
}