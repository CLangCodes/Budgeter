using Application.Abstractions.Messaging;
using Application.Expenses;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Expenses;

internal sealed class DeleteExpenseEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/v1/expenses/{expenseId:Guid}", async (
        Guid expenseId,
        ICommandHandler<DeleteExpenseCommand> handler,
        CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(
                new DeleteExpenseCommand(expenseId),
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Expenses)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
