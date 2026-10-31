using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Budgets;

public sealed record GetBudgetItemsQuery() : IQuery<GetBudgetItemsResult>;
public sealed record BudgetItemDetail(
    Guid BudgetItemId,
    Guid CategoryId,
    string BudgetItemName,
    string Description,
    RecurrenceUnit RecurrenceUnit,
    int RecurrenceInterval,
    DateOnly StartTrackingDate,
    DateOnly? EndDate
);
public sealed record GetBudgetItemsResult(List<BudgetItemDetail> BudgetItems);


internal sealed class GetBudgetItemsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetBudgetItemsQuery, GetBudgetItemsResult>
{
    public async Task<Result<GetBudgetItemsResult>> Handle(GetBudgetItemsQuery query, CancellationToken cancellationToken)
    {
        var budgetItems = await context.BudgetItems
            .AsNoTracking()
            .Select(e => new BudgetItemDetail(
                e.BudgetItemId,
                e.CategoryId,
                e.BudgetItemName,
                e.Description,
                e.RecurrenceUnit,
                e.RecurrenceInterval,
                e.StartTrackingDate,
                e.EndDate))
            .ToListAsync(cancellationToken);

        return new GetBudgetItemsResult(budgetItems);
    }
}
