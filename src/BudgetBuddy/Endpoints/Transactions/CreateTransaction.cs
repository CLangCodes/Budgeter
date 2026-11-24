using Domain.Models;
using Application.Abstractions.Authorization;
using Application.Abstractions.Messaging;
using Application.Transactions;
using BudgetBuddy.Extensions;
using BudgetBuddy.Http;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Transactions;

internal sealed class CreateTransactionEndpoint : IEndpoint
{
    public sealed record Request(
        TransactionType TransactionType,
        Guid AccountId,
        string Description,
        decimal Amount,
        DateOnly Date,
        Guid? CategoryId);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/transactions", async (
        Request request,
        ICommandHandler<CreateTransactionCommand, int> handler,
        CancellationToken cancellationToken) =>
        {
            var command = new CreateTransactionCommand(
                request.TransactionType,
                request.AccountId,
                request.Description,
                request.Amount,
                request.Date,
                request.CategoryId);

            Result<int> result = await handler.Handle(
                command,
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Permissions.TransactionsWrite)
        .WithTags(Tags.Transactions)
        .Produces<int>()
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
