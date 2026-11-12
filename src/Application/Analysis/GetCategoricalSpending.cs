using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Analysis;

public sealed record GetCategoricalSpendingQuery(
    Guid AccountId,
    DateTime StartDate,
    DateTime EndDate
) : IQuery<CategoricalSpendingResult>;

public sealed record CategoricalSpending(
    string CategoryName,
    decimal TotalAmount
);
public sealed record CategoricalSpendingResult(
    List<CategoricalSpending> SpendingByCategory
);

internal class GetCategoricalSpendingQueryHandler(IApplicationDbContext context) 
    : IQueryHandler<GetCategoricalSpendingQuery, CategoricalSpendingResult>
{
    public async Task<Result<CategoricalSpendingResult>> Handle(GetCategoricalSpendingQuery query, CancellationToken cancellationToken)
    {
        var spendingByCategory = await context.Transactions
            .Where(t => t.AccountId == query.AccountId && t.Date >= query.StartDate && t.Date <= query.EndDate)
            .GroupBy(t => t.Category.CategoryName)
            .Select(g => new CategoricalSpending(
                CategoryName: g.Key ?? "Uncategorized",
                TotalAmount: g.Sum(t => t.Amount)
            ))
            .ToListAsync(cancellationToken);

        return new CategoricalSpendingResult(spendingByCategory);
    }
}
