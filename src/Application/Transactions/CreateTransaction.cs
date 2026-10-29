using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using SharedKernel;

namespace Application.Transactions;

public sealed record CreateTransactionCommand(
    int TransactionTypeId,
    string Description,
    decimal Amount,
    DateTime Date,
    string Category) : ICommand<int>;

internal sealed class CreateTransactionCommandHandler(
    IApplicationDbContext context
) : ICommandHandler<CreateTransactionCommand, int>
{
    public async Task<Result<int>> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
    {
        Transaction transaction = Transaction.Create(
            command.TransactionTypeId,
            command.Description ?? string.Empty,
            command.Amount,
            command.Date,
            command.Category
        );

        context.Transactions.Add(transaction);
        await context.SaveChangesAsync(cancellationToken);
        return transaction.Id;
    }
}