using SharedKernel;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Application.Abstractions.Data;

namespace Application.Transactions;

public sealed record GetTransactionsQuery() : IQuery<GetTransactionsResult>;
public sealed record TransactionDetail(int Id, int TransactionTypeId, string Description, decimal Amount, DateTime Date, string Category);
public sealed record GetTransactionsResult(List<TransactionDetail> TransactionDetails);

internal class GetTransactionQueryHandler(IApplicationDbContext context) : IQueryHandler<GetTransactionsQuery, GetTransactionsResult>
{
    public async Task<Result<GetTransactionsResult>> Handle(GetTransactionsQuery query, CancellationToken cancellationToken)
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

        return new GetTransactionsResult(transactions);
    }
}