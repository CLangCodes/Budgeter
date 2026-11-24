using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Budgets;

public sealed record UpdateBudgetItemCommand(
    Guid BudgetItemId,
    Guid CategoryId,
    Guid? AccountId,
    string BudgetItemName,
    string Description,
    RecurrenceUnit RecurrenceUnit,
    int RecurrenceInterval,
    DateOnly StartTrackingDate,
    DateOnly? EndDate,
    decimal Amount
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

        bool categoryExists = await context.Categories
            .AnyAsync(c => c.CategoryId == command.CategoryId, cancellationToken);
        if (!categoryExists)
            {return Result.Failure(CategoryErrors.NotFound(command.CategoryId));}

        Result update = budgetItem.Update(
            command.CategoryId,
            command.AccountId,
            command.BudgetItemName,
            command.Description ?? string.Empty,
            command.RecurrenceUnit,
            command.RecurrenceInterval,
            command.StartTrackingDate,
            command.EndDate,
            command.Amount);
        if (update.IsFailure)
            {return update;}

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
