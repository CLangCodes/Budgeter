using SharedKernel;
using Infrastructure.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Application.Transactions;

public sealed record GetTransactionsQuery() : IQuery<GetTransactionsResult>;
public sealed record TransactionDetail(int Id, int TransactionTypeId, string Description, decimal Amount, DateTime Date, string Category);
public sealed record GetTransactionsResult(List<TransactionDetail> TransactionDetails, DateTime QueryTime);

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