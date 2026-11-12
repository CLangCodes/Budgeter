using Application.Categories;
using Application.Abstractions.Messaging;
using Application.Analysis;

namespace BudgetBuddy.Endpoints.Analysis;

internal sealed class GetCategoricalSpendingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/analysis/{accountId}/categorical-spending", 
        async (HttpContext httpContext,
        Guid accountId,
        DateTime? startDate,
        DateTime? endDate,
        IQueryHandler<GetCategoricalSpendingQuery, CategoricalSpendingResult> handler,
        CancellationToken cancellationToken) =>
        {
            var end = endDate ?? DateTime.UtcNow;
            var start = startDate ?? end.AddDays(-30);
            try
            {
                var result = await handler.Handle(
                    new GetCategoricalSpendingQuery(accountId, start, end), cancellationToken).ConfigureAwait(false);
                return Results.Ok(result.Value);
            } catch
            {
                return Results.Problem("There was a problem retrieving Categorical Spending data.");
            }
        })
        .Accepts<GetCategoricalSpendingQuery>("application/json")
        .WithTags(Tags.Analysis)
        .Produces<CategoricalSpendingResult>();
    }
}