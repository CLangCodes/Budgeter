using Application.Abstractions.Authorization;
using Application.Abstractions.Messaging;
using Application.Accounts;
using BudgetBuddy.Extensions;
using BudgetBuddy.Http;
using Domain.Models;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Accounts;

internal sealed class CreateAccountEndpoint : IEndpoint
{
    public sealed record Request(
        string NickName,
        AccountType AccountType,
        decimal StartingBalance);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/v1/accounts", async (
        Request request,
        ICommandHandler<CreateAccountCommand, Guid> handler,
        CancellationToken cancellationToken) =>
        {
            var command = new CreateAccountCommand(
                request.NickName,
                request.AccountType,
                request.StartingBalance);

            Result<Guid> result = await handler.Handle(
                command,
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Permissions.AccountsWrite)
        .WithTags(Tags.Accounts)
        .Produces<Guid>()
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
