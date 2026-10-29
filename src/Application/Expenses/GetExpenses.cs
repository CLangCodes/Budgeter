using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Expenses;

public sealed record GetExpensesQuery() : IQuery<GetExpensesResult>;
public sealed record ExpenseDetail(
    Guid ExpenseId,
    Guid CategoryId,
    string ExpenseName,
    string Description,
    RecurrenceUnit RecurrenceUnit,
    int RecurrenceInterval,
    DateOnly StartTrackingDate,
    DateOnly? EndDate
);
public sealed record GetExpensesResult(List<ExpenseDetail> Expenses);


internal sealed class GetExpensesQueryHandler(IApplicationDbContext context) : IQueryHandler<GetExpensesQuery, GetExpensesResult>
{
    public async Task<Result<GetExpensesResult>> Handle(GetExpensesQuery query, CancellationToken cancellationToken)
    {
        var expenses = await context.Expenses
            .AsNoTracking()
            .Select(e => new ExpenseDetail(
                e.ExpenseId,
                e.CategoryId,
                e.ExpenseName,
                e.Description,
                e.RecurrenceUnit,
                e.RecurrenceInterval,
                e.StartTrackingDate,
                e.EndDate))
            .ToListAsync(cancellationToken);

        return new GetExpensesResult(expenses);
    }
}
