using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using SharedKernel;

namespace Application.Accounts;

public sealed record CreateAccountCommand(
    string NickName,
    AccountType AccountType,
    decimal StartingBalance) : ICommand<Guid>;

internal sealed class CreateAccountCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider
) : ICommandHandler<CreateAccountCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        Account account = Account.Create(
            command.NickName ?? string.Empty,
            command.AccountType,
            command.StartingBalance,
            dateTimeProvider.UtcNow);

        context.Accounts.Add(account);
        await context.SaveChangesAsync(cancellationToken);
        return account.AccountId;
    }
}
