using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Expenses;

public sealed record GetExpenseByIdQuery(Guid ExpenseId) : IQuery<GetExpenseResult>;
public sealed record GetExpenseResult(
    Guid ExpenseId,
    Guid CategoryId,
    string ExpenseName,
    string Description,
    RecurrenceUnit RecurrenceUnit,
    int RecurrenceInterval,
    DateOnly StartTrackingDate,
    DateOnly? EndDate);

internal sealed class GetExpenseByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetExpenseByIdQuery, GetExpenseResult>
{
    public async Task<Result<GetExpenseResult>> Handle(GetExpenseByIdQuery query, CancellationToken cancellationToken)
    {
        GetExpenseResult? expense = await context.Expenses
            .Where(e => e.ExpenseId == query.ExpenseId)
            .Select(e => new GetExpenseResult(
                e.ExpenseId,
                e.CategoryId,
                e.ExpenseName,
                e.Description,
                e.RecurrenceUnit,
                e.RecurrenceInterval,
                e.StartTrackingDate,
                e.EndDate
            ))
            .SingleOrDefaultAsync(cancellationToken);
        if (expense is null)
        {
            return Result.Failure<GetExpenseResult>(ExpenseErrors.NotFound(query.ExpenseId));
        }
        return expense;
    }
}
