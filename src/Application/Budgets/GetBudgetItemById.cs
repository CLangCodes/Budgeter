using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Budgets;

public sealed record GetBudgetItemByIdQuery(Guid BudgetItemId, Guid? AccountId) : IQuery<GetBudgetItemResult>;
public sealed record GetBudgetItemResult(
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
    DateOnly? EndDate);

internal sealed class GetBudgetItemByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetBudgetItemByIdQuery, GetBudgetItemResult>
{
    public async Task<Result<GetBudgetItemResult>> Handle(GetBudgetItemByIdQuery query, CancellationToken cancellationToken)
    {
        GetBudgetItemResult? budgetItem = await context.BudgetItems
            .Where(e => e.BudgetItemId == query.BudgetItemId)
            .Include(b => b.Category)
            .Select(e => new GetBudgetItemResult(
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
                e.EndDate
            ))
            .SingleOrDefaultAsync(cancellationToken);
        if (budgetItem is null)
        {
            return Result.Failure<GetBudgetItemResult>(BudgetItemErrors.NotFound(query.BudgetItemId));
        }
        return budgetItem;
    }
}
