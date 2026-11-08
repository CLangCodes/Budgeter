using Application.Abstractions.Messaging;
using Application.Accounts;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Accounts;

internal sealed class UpdateAccountBalanceEndpoint : IEndpoint
{
    internal sealed record UpdateAccountBalanceRequest(
        decimal UpdatedAccountBalance);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/v1/accounts/{accountId:Guid}/balance", async (
        Guid accountId,
        [FromBody]
        UpdateAccountBalanceRequest request,
        ICommandHandler<UpdateAccountBalanceCommand> handler,
        CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(
                new UpdateAccountBalanceCommand(
                    accountId,
                    request.UpdatedAccountBalance
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
