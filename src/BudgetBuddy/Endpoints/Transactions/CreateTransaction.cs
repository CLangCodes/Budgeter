using Application.Abstractions.Messaging;
using Application.Transactions;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Transactions;

internal sealed class CreateTransactionEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/transactions", async (
        CreateTransactionCommand command,
        ICommandHandler<CreateTransactionCommand, int> handler, 
        CancellationToken cancellationToken) =>
        {
            Result<int> result = await handler.Handle(
                command, 
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .Accepts<CreateTransactionCommand>("Transactions")
        .WithTags(Tags.Transactions)
        .Produces<int>()
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
