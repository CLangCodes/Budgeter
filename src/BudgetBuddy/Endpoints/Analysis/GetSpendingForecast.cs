using Application.Abstractions.Messaging;
using Application.Analysis;

namespace BudgetBuddy.Endpoints.Analysis;

internal sealed class GetSpendingForecastEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/analysis/{accountId}/spending-forecast", 
        async (HttpContext httpContext,
        Guid accountId,
        DateOnly? startDate,
        DateOnly? endDate,
        IQueryHandler<GetSpendingForecastQuery, GetSpendingForecastResult> handler,
        CancellationToken cancellationToken) =>
        {
            var end = endDate ?? DateOnly.FromDateTime(DateTime.UtcNow);
            var start = startDate ?? end.AddDays(-30);
            try
            {
                var result = await handler.Handle(
                    new GetSpendingForecastQuery(accountId, start, end), cancellationToken).ConfigureAwait(false);
                return Results.Ok(result.Value);
            } catch
            {
                return Results.Problem("There was a problem retrieving Spending Forecast data.");
            }
        })
        .Accepts<GetSpendingForecastQuery>("application/json")
        .WithTags(Tags.Analysis)
        .Produces<GetSpendingForecastResult>();
    }
}