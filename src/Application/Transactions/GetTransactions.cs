using SharedKernel;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Application.Abstractions.Data;

namespace Application.Transactions;

public sealed record GetTransactionsQuery() : IQuery<GetTransactionsResult>;
public sealed record GetTransactionsResult(List<TransactionDetail> TransactionDetails);

internal class GetTransactionQueryHandler(IApplicationDbContext context) : IQueryHandler<GetTransactionsQuery, GetTransactionsResult>
{
    public async Task<Result<GetTransactionsResult>> Handle(GetTransactionsQuery query, CancellationToken cancellationToken)
    {
        var transactions = await context.Transactions
            .AsNoTracking()
            .Select(t => new TransactionDetail(
                t.Id,
                t.AccountId ?? Guid.Empty,
                t.TransactionTypeId,
                t.Description,
                t.Amount,
                t.Date,
                t.CategoryId,
                t.Category!.CategoryName ?? string.Empty,
                t.Category.Description))
            .ToListAsync(cancellationToken);

        return new GetTransactionsResult(transactions);
    }
}