using Application.Abstractions.Messaging;
using Application.Expenses;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Expenses;

internal sealed class GetExpensesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/expenses", async (
        IQueryHandler<GetExpensesQuery, GetExpensesResult> handler,
        CancellationToken cancellationToken) =>
        {
            Result<GetExpensesResult> result = await handler.Handle(
                new GetExpensesQuery(), cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Expenses)
        .Produces<GetExpensesResult>();
    }
}
