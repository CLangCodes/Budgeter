using Application.Abstractions.Messaging;
using Application.Budgets;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Budgets;

internal sealed class GetBudgetItemByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/budgets/{budgetItemId:Guid}", async (
        Guid budgetItemId,
        IQueryHandler<GetBudgetItemByIdQuery, GetBudgetItemResult> handler,
        CancellationToken cancellationToken) =>
        {
            Result<GetBudgetItemResult> result = await handler.Handle(
                new GetBudgetItemByIdQuery(budgetItemId), cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Budgets)
        .Produces<GetBudgetItemResult>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
