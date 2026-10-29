using BudgetBuddy.Endpoints;
using Application.Transactions;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Transactions;

internal sealed class GetTransactionByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/v1/transactions/{transactionId:int}", async (HttpContext httpContext,
        int transactionId,
        IQueryHandler<GetTransactionByIdQuery, GetTransactionResult> handler,
        CancellationToken cancellationToken) =>
        {
            try
            {
                var result = await handler.Handle(
                    new GetTransactionByIdQuery(transactionId), cancellationToken).ConfigureAwait(false);
                return Results.Ok(result.Value);                
            } catch
            {
                return Results.Problem("There was a problem retrieving Transactions.");
            }
        })
        .WithTags(Tags.Transactions)
        .Accepts<GetTransactionByIdQuery>("Transactions")
        .Produces<GetTransactionResult>()
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}