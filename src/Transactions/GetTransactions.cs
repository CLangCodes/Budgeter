using BudgetBuddy.Kernel;
using BudgetBuddy.Kernel.Api;
using BudgetBuddy.Data;
using BudgetBuddy.Kernel.Messaging;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Transactions;

internal sealed record GetTransactionsQuery() : IQuery<GetTransactionsResult>;
internal sealed record TransactionDetail(int Id, int TransactionTypeId, string Description, decimal Amount, DateTime Date, string Category);
internal sealed record GetTransactionsResult(List<TransactionDetail> TransactionDetails, DateTime QueryTime);

internal class GetTransactionQueryHandler : IQueryHandler<GetTransactionsQuery, GetTransactionsResult>
{
    private readonly IDbContextFactory<BBDbContext> _factory;
    private readonly IDateTimeProvider _timeProvider;
    public GetTransactionQueryHandler(IDbContextFactory<BBDbContext> factory, IDateTimeProvider timeProvider)
    {
        _factory = factory;
        _timeProvider = timeProvider;
    }
    public async Task<Result<GetTransactionsResult>> Handle(GetTransactionsQuery query, CancellationToken cancellationToken)
    {
        var context = await _factory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
        await using (context.ConfigureAwait(false))
        {
            var transactions = await context.Transactions
                .AsNoTracking()
                .Select(t => new TransactionDetail(
                    t.Id,
                    t.TransactionTypeId,
                    t.Description,
                    t.Amount,
                    t.Date,
                    t.Category))
                .ToListAsync(cancellationToken);

            return new GetTransactionsResult(transactions, _timeProvider.UtcNow);
        }
    }
}

internal sealed class GetTransactionsEndpoint : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
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
        });
    }
}

internal sealed class TransactionsNotFoundException : Exception
{
    public TransactionsNotFoundException() : base () { }
    public TransactionsNotFoundException(string message) : base(message) { }
    public TransactionsNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}