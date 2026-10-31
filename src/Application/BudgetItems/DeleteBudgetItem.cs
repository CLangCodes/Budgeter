using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Budgets;

public sealed record DeleteBudgetItemCommand(Guid BudgetItemId) : ICommand;
internal sealed class DeleteBudgetItemCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteBudgetItemCommand>
{
    public async Task<Result> Handle(DeleteBudgetItemCommand command, CancellationToken cancellationToken)
    {
        BudgetItem? budgetItem = await context.BudgetItems.SingleOrDefaultAsync(
            e => e.BudgetItemId == command.BudgetItemId, cancellationToken);

        if (budgetItem is null)
            {return Result.Failure(BudgetItemErrors.NotFound(command.BudgetItemId));}
        context.BudgetItems.Remove(budgetItem);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
