using SharedKernel;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Application.Abstractions.Data;

namespace Application.Transactions;

public sealed record GetTransactionsInDateRangeQuery(DateTime StartDate, DateTime EndDate) : IQuery<GetTransactionsInDateRangeResult>;
public sealed record GetTransactionsInDateRangeResult(List<TransactionDetail> TransactionDetails);

internal class GetTransactionsInDateRangeQueryHandler(IApplicationDbContext context) : IQueryHandler<GetTransactionsInDateRangeQuery, GetTransactionsInDateRangeResult>
{
    public async Task<Result<GetTransactionsInDateRangeResult>> Handle(GetTransactionsInDateRangeQuery query, CancellationToken cancellationToken)
    {
        var transactions = await context.Transactions
            .AsNoTracking()
            .Where(t => t.Date >= query.StartDate && t.Date <= query.EndDate)
            .Select(t => new TransactionDetail(
                t.Id,
                t.AccountId ?? Guid.Empty,
                t.TransactionTypeId,
                t.Description,
                t.Amount,
                t.Date,
                t.Category))
            .ToListAsync(cancellationToken);

        return new GetTransactionsInDateRangeResult(transactions);
    }
}