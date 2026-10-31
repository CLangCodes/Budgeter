using Application.Abstractions.Messaging;
using Application.Budgets;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Budgets;

internal sealed class DeleteBudgetItemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/v1/budgets/{budgetItemId:Guid}", async (
        Guid budgetItemId,
        ICommandHandler<DeleteBudgetItemCommand> handler,
        CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(
                new DeleteBudgetItemCommand(budgetItemId),
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Budgets)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
