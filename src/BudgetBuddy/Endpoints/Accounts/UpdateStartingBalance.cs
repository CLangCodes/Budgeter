using Application.Abstractions.Messaging;
using Application.Accounts;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Accounts;

internal sealed class UpdateStartingBalanceEndpoint : IEndpoint
{
    internal sealed record UpdateStartingBalanceRequest(
        decimal UpdatedStartingBalance);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/v1/accounts/{accountId:Guid}/balance", async (
        Guid accountId,
        [FromBody]
        UpdateStartingBalanceRequest request,
        ICommandHandler<UpdateStartingBalanceCommand> handler,
        CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(
                new UpdateStartingBalanceCommand(
                    accountId,
                    request.UpdatedStartingBalance
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
