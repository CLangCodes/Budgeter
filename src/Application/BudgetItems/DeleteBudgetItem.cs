using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Expenses;

public sealed record DeleteExpenseCommand(Guid ExpenseId) : ICommand;
internal sealed class DeleteExpenseCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteExpenseCommand>
{
    public async Task<Result> Handle(DeleteExpenseCommand command, CancellationToken cancellationToken)
    {
        Expense? expense = await context.Expenses.SingleOrDefaultAsync(
            e => e.ExpenseId == command.ExpenseId, cancellationToken);

        if (expense is null)
            {return Result.Failure(ExpenseErrors.NotFound(command.ExpenseId));}
        context.Expenses.Remove(expense);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
