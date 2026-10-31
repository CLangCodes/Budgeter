using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Expenses;

public sealed record CreateExpenseCommand(
    Guid CategoryId,
    string ExpenseName,
    string Description,
    RecurrenceUnit RecurrenceUnit,
    int RecurrenceInterval,
    DateOnly StartTrackingDate,
    DateOnly? EndDate) : ICommand<Guid>;

internal sealed class CreateExpenseCommandHandler(
    IApplicationDbContext context
) : ICommandHandler<CreateExpenseCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateExpenseCommand command, CancellationToken cancellationToken)
    {
        Result recurrence = Expense.ValidateRecurrence(
            command.RecurrenceInterval,
            command.StartTrackingDate,
            command.EndDate);
        if (recurrence.IsFailure)
            {return Result.Failure<Guid>(recurrence.Error);}

        bool categoryExists = await context.Categories
            .AnyAsync(c => c.CategoryId == command.CategoryId, cancellationToken);
        if (!categoryExists)
            {return Result.Failure<Guid>(CategoryErrors.NotFound(command.CategoryId));}

        Expense expense = Expense.Create(
            command.CategoryId,
            command.ExpenseName,
            command.Description ?? string.Empty,
            command.RecurrenceUnit,
            command.RecurrenceInterval,
            command.StartTrackingDate,
            command.EndDate
        );

        context.Expenses.Add(expense);
        await context.SaveChangesAsync(cancellationToken);
        return expense.ExpenseId;
    }
}
