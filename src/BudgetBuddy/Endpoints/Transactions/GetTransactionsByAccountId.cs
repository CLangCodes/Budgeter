using Application.Transactions;
using Application.Abstractions.Messaging;

namespace BudgetBuddy.Endpoints.Transactions;

internal sealed class GetTransactionsByAccountIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/transactions/accounts/{accountId}", async (Guid accountId, HttpContext httpContext,
        IQueryHandler<GetTransactionsByAccountIdQuery, GetTransactionsByAccountIdResult> handler,
        CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await handler.Handle(
                    new GetTransactionsByAccountIdQuery(accountId), cancellationToken).ConfigureAwait(false);
                return Results.Ok(result.Value);                
            } catch
            {
                return Results.Problem("There was a problem retrieving Transactions.");
            }
        })
        .Accepts<GetTransactionsByAccountIdQuery>("application/json")
        .WithTags(Tags.Transactions)
        .Produces<GetTransactionsByAccountIdResult>();
    }
}