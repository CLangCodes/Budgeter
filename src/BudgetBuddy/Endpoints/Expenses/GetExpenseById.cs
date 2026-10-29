using Application.Abstractions.Messaging;
using Application.Expenses;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Expenses;

internal sealed class GetExpenseByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/expenses/{expenseId:Guid}", async (
        Guid expenseId,
        IQueryHandler<GetExpenseByIdQuery, GetExpenseResult> handler,
        CancellationToken cancellationToken) =>
        {
            Result<GetExpenseResult> result = await handler.Handle(
                new GetExpenseByIdQuery(expenseId), cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Expenses)
        .Produces<GetExpenseResult>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
