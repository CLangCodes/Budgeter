using Domain.Models;
using Application.Abstractions.Authorization;
using Application.Abstractions.Messaging;
using Application.Transactions;
using BudgetBuddy.Extensions;
using BudgetBuddy.Http;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Transactions;

internal sealed class UpdateTransactionEndpoint : IEndpoint
{
    internal sealed record UpdateTransactionRequest(
        TransactionType TransactionType,
        Guid AccountId,
        string Description,
        decimal Amount,
        DateOnly Date,
        Guid? CategoryId);

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
                    request.TransactionType,
                    request.Description,
                    request.Amount,
                    request.Date,
                    request.CategoryId
                    ),
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .HasPermission(Permissions.TransactionsWrite)
        .WithTags(Tags.Transactions)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
