using Application.Abstractions.Authorization;
using Application.Abstractions.Messaging;
using Application.Transactions;
using BudgetBuddy.Extensions;
using BudgetBuddy.Http;
using SharedKernel;

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
            // TODO: move to the Application layer with the upcoming validation work.
            if (startDate > endDate)
                return Results.BadRequest("startDate must be before endDate.");

            Result<GetTransactionsResult> result = await handler.Handle(
                new GetTransactionsQuery(accountId, startDate, endDate), cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Permissions.TransactionsRead)
        .WithTags(Tags.Transactions)
        .Produces<GetTransactionsResult>()
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
