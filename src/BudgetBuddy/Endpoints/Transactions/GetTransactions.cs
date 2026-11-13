using Application.Transactions;
using Application.Abstractions.Messaging;

namespace BudgetBuddy.Endpoints.Transactions;

internal sealed class GetTransactionsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/transactions", async (
        Guid? accountId,
        DateOnly? startDate,
        DateOnly? endDate,
        IQueryHandler<GetTransactionsQuery, GetTransactionsResult> handler,
        CancellationToken cancellationToken) =>
        {
            if (startDate > endDate)
                return Results.BadRequest("startDate must be before endDate.");

            var result = await handler.Handle(
                new GetTransactionsQuery(accountId, startDate, endDate), cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.Problem("There was a problem retrieving Transactions.");
        })
        .WithTags(Tags.Transactions)
        .Produces<GetTransactionsResult>()
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
