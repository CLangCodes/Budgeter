namespace BudgetBuddy.Extensions;

internal static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddOpenApiWithAuth(this IServiceCollection services)
    {
        services.AddOpenApi(options =>
        {
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