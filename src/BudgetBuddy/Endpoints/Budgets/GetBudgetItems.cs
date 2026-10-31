using Application.Abstractions.Messaging;
using Application.Budgets;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Budgets;

internal sealed class GetBudgetItemsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/budgets", async (
        IQueryHandler<GetBudgetItemsQuery, GetBudgetItemsResult> handler,
        CancellationToken cancellationToken) =>
        {
            Result<GetBudgetItemsResult> result = await handler.Handle(
                new GetBudgetItemsQuery(), cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Budgets)
        .Produces<GetBudgetItemsResult>();
    }
}
