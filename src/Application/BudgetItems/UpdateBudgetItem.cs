using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Budgets;

public sealed record UpdateBudgetItemCommand(
    Guid BudgetItemId,
    Guid CategoryId,
    string BudgetItemName,
    string Description,
    RecurrenceUnit RecurrenceUnit,
    int RecurrenceInterval,
    DateOnly StartTrackingDate,
    DateOnly? EndDate
) : ICommand;

internal sealed class UpdateBudgetItemCommandHandler(IApplicationDbContext context
    ) : ICommandHandler<UpdateBudgetItemCommand>
{
    public async Task<Result> Handle(UpdateBudgetItemCommand command, CancellationToken cancellationToken)
    {
        var budgetItem = await context.BudgetItems
            .SingleOrDefaultAsync(e => e.BudgetItemId == command.BudgetItemId, cancellationToken);

        if (budgetItem is null)
            {return Result.Failure(BudgetItemErrors.NotFound(command.BudgetItemId));}

        Result recurrence = BudgetItem.ValidateRecurrence(
            command.RecurrenceInterval,
            command.StartTrackingDate,
            command.EndDate);
        if (recurrence.IsFailure)
            {return recurrence;}

        bool categoryExists = await context.Categories
            .AnyAsync(c => c.CategoryId == command.CategoryId, cancellationToken);
        if (!categoryExists)
            {return Result.Failure(CategoryErrors.NotFound(command.CategoryId));}

        budgetItem.Update(
            command.CategoryId,
            command.BudgetItemName,
            command.Description ?? string.Empty,
            command.RecurrenceUnit,
            command.RecurrenceInterval,
            command.StartTrackingDate,
            command.EndDate);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
