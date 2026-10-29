using Application.Transactions;
using Application.Abstractions.Messaging;

namespace BudgetBuddy.Endpoints.Transactions;

internal sealed class GetTransactionsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/transactions/", async (HttpContext httpContext,
        IQueryHandler<GetTransactionsQuery, GetTransactionsResult> handler,
        CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await handler.Handle(
                    new GetTransactionsQuery(), cancellationToken).ConfigureAwait(false);
                return Results.Ok(result.Value);                
            } catch
            {
                return Results.Problem("There was a problem retrieving Transactions.");
            }
        })
        .Accepts<GetTransactionsQuery>("Transactions")
        .WithTags(Tags.Transactions)
        .Produces<GetTransactionsResult>();
    }
}