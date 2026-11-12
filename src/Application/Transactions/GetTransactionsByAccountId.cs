using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Transactions;

public sealed record GetTransactionsByAccountIdQuery(Guid AccountId) : IQuery<GetTransactionsByAccountIdResult>;
public sealed record GetTransactionsByAccountIdResult(List<TransactionDetail> Transactions);

internal class GetTransactionsByAccountIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetTransactionsByAccountIdQuery, GetTransactionsByAccountIdResult>
{
    public async Task<Result<GetTransactionsByAccountIdResult>> Handle(
        GetTransactionsByAccountIdQuery request, 
        CancellationToken cancellationToken)
    {
        var transactions = await context.Transactions
            .AsNoTracking()
            .Where(t => t.AccountId == request.AccountId)
            .Include(t => t.Category)
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

        return new GetTransactionsByAccountIdResult(transactions);
    }
}