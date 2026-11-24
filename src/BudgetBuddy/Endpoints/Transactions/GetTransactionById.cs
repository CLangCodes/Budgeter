using Application.Abstractions.Authorization;
using Application.Abstractions.Messaging;
using Application.Transactions;
using BudgetBuddy.Extensions;
using BudgetBuddy.Http;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Transactions;

internal sealed class GetTransactionByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/transactions/{transactionId:int}", async (
        int transactionId,
        IQueryHandler<GetTransactionByIdQuery, GetTransactionResult> handler,
        CancellationToken cancellationToken) =>
        {
            Result<GetTransactionResult> result = await handler.Handle(
                new GetTransactionByIdQuery(transactionId), cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Permissions.TransactionsRead)
        .WithTags(Tags.Transactions)
        .Produces<GetTransactionResult>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
