using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Transactions;

public sealed record DeleteTransactionCommand(int TransactionId) : ICommand;
internal sealed class DeleteTransactionCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteTransactionCommand>
{
    public async Task<Result> Handle(DeleteTransactionCommand command, CancellationToken cancellationToken)
    {
        Transaction? transaction = await context.Transactions.SingleOrDefaultAsync(
            g => g.Id == command.TransactionId, cancellationToken);

        if (transaction is null)
            {return Result.Failure(TransactionErrors.NotFound(command.TransactionId));}
        context.Transactions.Remove(transaction);
        
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}