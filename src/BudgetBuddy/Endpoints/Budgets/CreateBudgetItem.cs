using Application.Abstractions.Messaging;
using Application.Expenses;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Expenses;

internal sealed class CreateExpenseEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/expenses", async (
        CreateExpenseCommand command,
        ICommandHandler<CreateExpenseCommand, Guid> handler,
        CancellationToken cancellationToken) =>
        {
            Result<Guid> result = await handler.Handle(
                command,
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .Accepts<CreateExpenseCommand>("application/json")
        .WithTags(Tags.Expenses)
        .Produces<Guid>()
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
