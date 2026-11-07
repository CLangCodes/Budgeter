using Application.Abstractions.Messaging;
using Application.Accounts;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Accounts;

internal sealed class GetAccountsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/accounts", async (
        IQueryHandler<GetAccountsQuery, GetAccountsResult> handler,
        CancellationToken cancellationToken) =>
        {
            Result<GetAccountsResult> result = await handler.Handle(
                new GetAccountsQuery(), cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Accounts)
        .Produces<GetAccountsResult>();
    }
}
