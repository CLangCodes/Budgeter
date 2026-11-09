using Application.Transactions;
using Application.Abstractions.Messaging;

namespace BudgetBuddy.Endpoints.Transactions;

internal sealed class GetTransactionsInDateRangeEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/transactions", async (
        DateTime? startDate,
        DateTime? endDate,
        IQueryHandler<GetTransactionsInDateRangeQuery, GetTransactionsInDateRangeResult> handler,
        CancellationToken cancellationToken) =>
        {
            var end = endDate ?? DateTime.UtcNow;
            var start = startDate ?? end.AddDays(-30);

            if (start > end)
                return Results.BadRequest("startDate must be before endDate.");

            var result = await handler.Handle(
                new GetTransactionsInDateRangeQuery(start, end), cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.Problem("There was a problem retrieving Transactions.");
        })
        .WithTags(Tags.Transactions)
        .Produces<GetTransactionsInDateRangeResult>()
        .ProducesProblem(StatusCodes.Status400BadRequest);

    }
}