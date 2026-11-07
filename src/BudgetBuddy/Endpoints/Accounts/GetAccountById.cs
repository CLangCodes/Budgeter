using Application.Abstractions.Messaging;
using Application.Accounts;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Accounts;

internal sealed class GetAccountByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/accounts/{accountId:Guid}", async (
        Guid accountId,
        IQueryHandler<GetAccountByIdQuery, GetAccountResult> handler,
        CancellationToken cancellationToken) =>
        {
            Result<GetAccountResult> result = await handler.Handle(
                new GetAccountByIdQuery(accountId), cancellationToken).ConfigureAwait(false);
            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .WithTags(Tags.Accounts)
        .Produces<GetAccountResult>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}
