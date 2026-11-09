using Application.Abstractions.Messaging;
using Application.Accounts;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Accounts;

internal sealed class GetAccountBalanceEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/accounts/{accountId:Guid}/balance", async (
        Guid accountId,
        IQueryHandler<GetAccountBalanceQuery, GetAccountBalanceResult> handler,
        CancellationToken cancellationToken) =>
        {
            Result<GetAccountBalanceResult> result = await handler.Handle(
                new GetAccountBalanceQuery(accountId), cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Accounts)
        .Produces<GetAccountBalanceResult>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
