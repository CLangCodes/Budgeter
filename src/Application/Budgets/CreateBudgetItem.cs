using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Budgets;

public sealed record CreateBudgetItemCommand(
    Guid CategoryId,
    string BudgetItemName,
    string Description,
    RecurrenceUnit RecurrenceUnit,
    int RecurrenceInterval,
    DateOnly StartTrackingDate,
    DateOnly? EndDate) : ICommand<Guid>;

internal sealed class CreateBudgetItemCommandHandler(
    IApplicationDbContext context
) : ICommandHandler<CreateBudgetItemCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateBudgetItemCommand command, CancellationToken cancellationToken)
    {
        Result recurrence = BudgetItem.ValidateRecurrence(
            command.RecurrenceInterval,
            command.StartTrackingDate,
            command.EndDate);
        if (recurrence.IsFailure)
            {return Result.Failure<Guid>(recurrence.Error);}

        bool categoryExists = await context.Categories
            .AnyAsync(c => c.CategoryId == command.CategoryId, cancellationToken);
        if (!categoryExists)
            {return Result.Failure<Guid>(CategoryErrors.NotFound(command.CategoryId));}

        BudgetItem budgetItem = BudgetItem.Create(
            command.CategoryId,
            command.BudgetItemName,
            command.Description ?? string.Empty,
            command.RecurrenceUnit,
            command.RecurrenceInterval,
            command.StartTrackingDate,
            command.EndDate
        );

        context.BudgetItems.Add(budgetItem);
        await context.SaveChangesAsync(cancellationToken);
        return budgetItem.BudgetItemId;
    }
}
