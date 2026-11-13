using SharedKernel;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using Application.Abstractions.Data;

namespace Application.Transactions;

/// <summary>
/// Retrieves transactions, optionally filtered by account and/or an inclusive date range.
/// A null filter is not applied; a null date bound leaves that side of the range open.
/// </summary>
public sealed record GetTransactionsQuery(
    Guid? AccountId = null,
    DateOnly? StartDate = null,
    DateOnly? EndDate = null) : IQuery<GetTransactionsResult>;
public sealed record GetTransactionsResult(List<TransactionDetail> TransactionDetails);

internal sealed class GetTransactionsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetTransactionsQuery, GetTransactionsResult>
{
    public async Task<Result<GetTransactionsResult>> Handle(GetTransactionsQuery query, CancellationToken cancellationToken)
    {
        var transactions = context.Transactions.AsNoTracking();

        if (query.AccountId is { } accountId)
            transactions = transactions.Where(t => t.AccountId == accountId);

        if (query.StartDate is { } start)
            transactions = transactions.Where(t => t.Date >= start);

        if (query.EndDate is { } end)
            transactions = transactions.Where(t => t.Date <= end);

        List<TransactionDetail> details = await transactions
            .OrderByDescending(t => t.Date)
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

        return new GetTransactionsResult(details);
    }
}
