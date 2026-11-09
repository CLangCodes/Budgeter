using Application.Abstractions.Messaging;
using Application.Accounts;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Accounts;

internal sealed class DeleteAccountEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/v1/accounts/{accountId:Guid}", async (
        Guid accountId,
        ICommandHandler<DeleteAccountCommand> handler,
        CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(
                new DeleteAccountCommand(accountId),
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Accounts)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
