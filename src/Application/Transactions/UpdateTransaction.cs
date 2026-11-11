using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Transactions;

public sealed record UpdateTransactionCommand(
    int TransactionId,
    Guid AccountId,
    int TransactionTypeId, 
    string Description, 
    decimal Amount, 
    DateTime Date, 
    Guid? CategoryId,
    string CategoryName
) : ICommand;

internal sealed class UpdateTransactionCommandHandler(IApplicationDbContext context
    ) : ICommandHandler<UpdateTransactionCommand>
{
    public async Task<Result> Handle(UpdateTransactionCommand command, CancellationToken cancellationToken)
    {
        var transaction = await context.Transactions
            .SingleOrDefaultAsync(g => g.Id == command.TransactionId, cancellationToken);

        if (transaction is null)
            {return Result.Failure(TransactionErrors.NotFound(command.TransactionId));}
        transaction.Update(
            command.TransactionTypeId,
            command.AccountId,
            command.Description,
            command.Amount,
            command.Date,
            command.CategoryId,
            command.CategoryName
            );

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}