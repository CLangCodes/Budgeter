using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Transactions;

public sealed record UpdateTransactionCommand(
    int TransactionId,
    int TransactionTypeId, 
    string Description, 
    decimal Amount, 
    DateTime Date, 
    string Category
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
            command.Description,
            command.Amount,
            command.Date,
            command.Description
            );

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}