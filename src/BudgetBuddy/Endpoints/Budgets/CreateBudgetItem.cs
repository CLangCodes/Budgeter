using Application.Abstractions.Messaging;
using Application.Budgets;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Budgets;

internal sealed class CreateBudgetItemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/budgets", async (
        CreateBudgetItemCommand command,
        ICommandHandler<CreateBudgetItemCommand, Guid> handler,
        CancellationToken cancellationToken) =>
        {
            Result<Guid> result = await handler.Handle(
                command,
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .Accepts<CreateBudgetItemCommand>("application/json")
        .WithTags(Tags.Budgets)
        .Produces<Guid>()
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
