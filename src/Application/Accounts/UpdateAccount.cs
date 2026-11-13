using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Accounts;

public sealed record UpdateAccountCommand(
    Guid AccountId,
    string NickName,
    AccountType AccountType,
    decimal StartingBalance
) : ICommand;

internal sealed class UpdateAccountCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider
) : ICommandHandler<UpdateAccountCommand>
{
    public async Task<Result> Handle(UpdateAccountCommand command, CancellationToken cancellationToken)
    {
        Account? account = await context.Accounts
            .SingleOrDefaultAsync(a => a.AccountId == command.AccountId, cancellationToken);

        if (account is null)
            {return Result.Failure(AccountErrors.NotFound(command.AccountId));}

        account.Update(
            command.NickName ?? string.Empty,
            command.AccountType,
            command.StartingBalance,
            dateTimeProvider.UtcNowDateOnly);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
