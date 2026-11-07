using Application.Abstractions.Messaging;
using Application.Accounts;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Accounts;

internal sealed class UpdateAccountEndpoint : IEndpoint
{
    internal sealed record UpdateAccountRequest(
        string NickName,
        AccountType AccountType,
        decimal StartingBalance);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/v1/accounts/{accountId:Guid}", async (
        Guid accountId,
        [FromBody]
        UpdateAccountRequest request,
        ICommandHandler<UpdateAccountCommand> handler,
        CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(
                new UpdateAccountCommand(
                    accountId,
                    request.NickName,
                    request.AccountType,
                    request.StartingBalance
                    ),
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .WithTags(Tags.Accounts)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
