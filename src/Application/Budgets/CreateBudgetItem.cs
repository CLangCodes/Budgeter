using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Budgets;

public sealed record CreateBudgetItemCommand(
    Guid CategoryId,
    Guid? AccountId,
    string BudgetItemName,
    string Description,
    RecurrenceUnit RecurrenceUnit,
    int RecurrenceInterval,
    DateOnly StartTrackingDate,
    DateOnly? EndDate,
    decimal Amount) : ICommand<Guid>;

internal sealed class CreateBudgetItemCommandHandler(
    IApplicationDbContext context
) : ICommandHandler<CreateBudgetItemCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateBudgetItemCommand command, CancellationToken cancellationToken)
    {
        bool categoryExists = await context.Categories
            .AnyAsync(c => c.CategoryId == command.CategoryId, cancellationToken);
        if (!categoryExists)
            {return Result.Failure<Guid>(CategoryErrors.NotFound(command.CategoryId));}

        Result<BudgetItem> created = BudgetItem.Create(
            command.CategoryId,
            command.AccountId,
            command.BudgetItemName,
            command.Description ?? string.Empty,
            command.RecurrenceUnit,
            command.RecurrenceInterval,
            command.StartTrackingDate,
            command.EndDate,
            command.Amount
        );
        if (created.IsFailure)
            {return Result.Failure<Guid>(created.Error);}

        BudgetItem budgetItem = created.Value;
        context.BudgetItems.Add(budgetItem);
        await context.SaveChangesAsync(cancellationToken);
        return budgetItem.BudgetItemId;
    }
}
