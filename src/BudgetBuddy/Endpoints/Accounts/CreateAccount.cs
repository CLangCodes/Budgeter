using Application.Abstractions.Messaging;
using Application.Accounts;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Accounts;

internal sealed class CreateAccountEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/accounts", async (
        CreateAccountCommand command,
        ICommandHandler<CreateAccountCommand, Guid> handler,
        CancellationToken cancellationToken) =>
        {
            Result<Guid> result = await handler.Handle(
                command,
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .Accepts<CreateAccountCommand>("application/json")
        .WithTags(Tags.Accounts)
        .Produces<Guid>()
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
