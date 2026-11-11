using Application.Abstractions.Messaging;
using Application.Transactions;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Transactions;

internal sealed class UpdateTransactionEndpoint : IEndpoint
{
    internal sealed record UpdateTransactionRequest(
        int TransactionTypeId,
        Guid AccountId,
        string Description,
        decimal Amount,
        DateTime Date,
        Guid? CategoryId,
        string Category);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/v1/transactions/{transactionId:int}", async (
        int transactionId,
        [FromBody]
        UpdateTransactionRequest request,
        ICommandHandler<UpdateTransactionCommand> handler,
        CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(
                new UpdateTransactionCommand(
                    transactionId,
                    request.AccountId,
                    request.TransactionTypeId,
                    request.Description,
                    request.Amount,
                    request.Date,
                    request.CategoryId,
                    request.Category
                    ),
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .Accepts<UpdateTransactionCommand>("application/json")
        .WithTags(Tags.Transactions)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
