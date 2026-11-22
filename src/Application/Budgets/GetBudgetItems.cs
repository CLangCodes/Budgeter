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
    string CategoryName,
    string CategoryDescription,
    Guid? AccountId,
    string BudgetItemName,
    string Description,
    RecurrenceUnit RecurrenceUnit,
    int RecurrenceInterval,
    DateOnly StartTrackingDate,
    DateOnly? EndDate,
    decimal Amount
);
public sealed record GetBudgetItemsResult(List<BudgetItemDetail> BudgetItems);


internal sealed class GetBudgetItemsQueryHandler(IApplicationDbContext context) : IQueryHandler<GetBudgetItemsQuery, GetBudgetItemsResult>
{
    public async Task<Result<GetBudgetItemsResult>> Handle(GetBudgetItemsQuery query, CancellationToken cancellationToken)
    {
        var budgetItems = await context.BudgetItems
            .AsNoTracking()
            .Include(b => b.Category)
            .Select(e => new BudgetItemDetail(
                e.BudgetItemId,
                e.CategoryId,
                e.Category!.CategoryName ?? string.Empty,
                e.Category.Description ?? string.Empty,
                e.AccountId,
                e.BudgetItemName,
                e.Description,
                e.RecurrenceUnit,
                e.RecurrenceInterval,
                e.StartTrackingDate,
                e.EndDate,
                e.Amount))
            .ToListAsync(cancellationToken);

        return new GetBudgetItemsResult(budgetItems);
    }
}
