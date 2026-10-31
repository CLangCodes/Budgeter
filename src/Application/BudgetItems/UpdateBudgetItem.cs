using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Expenses;

public sealed record UpdateExpenseCommand(
    Guid ExpenseId,
    Guid CategoryId,
    string ExpenseName,
    string Description,
    RecurrenceUnit RecurrenceUnit,
    int RecurrenceInterval,
    DateOnly StartTrackingDate,
    DateOnly? EndDate
) : ICommand;

internal sealed class UpdateExpenseCommandHandler(IApplicationDbContext context
    ) : ICommandHandler<UpdateExpenseCommand>
{
    public async Task<Result> Handle(UpdateExpenseCommand command, CancellationToken cancellationToken)
    {
        var expense = await context.Expenses
            .SingleOrDefaultAsync(e => e.ExpenseId == command.ExpenseId, cancellationToken);

        if (expense is null)
            {return Result.Failure(ExpenseErrors.NotFound(command.ExpenseId));}

        Result recurrence = Expense.ValidateRecurrence(
            command.RecurrenceInterval,
            command.StartTrackingDate,
            command.EndDate);
        if (recurrence.IsFailure)
            {return recurrence;}

        bool categoryExists = await context.Categories
            .AnyAsync(c => c.CategoryId == command.CategoryId, cancellationToken);
        if (!categoryExists)
            {return Result.Failure(CategoryErrors.NotFound(command.CategoryId));}

        expense.Update(
            command.CategoryId,
            command.ExpenseName,
            command.Description ?? string.Empty,
            command.RecurrenceUnit,
            command.RecurrenceInterval,
            command.StartTrackingDate,
            command.EndDate);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
