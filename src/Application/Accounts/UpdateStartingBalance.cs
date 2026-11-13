using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Accounts;

public sealed record UpdateStartingBalanceCommand(
    Guid AccountId,
    decimal UpdateStartingBalance
) : ICommand;

internal sealed class UpdateAccountBalanceCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider
) : ICommandHandler<UpdateStartingBalanceCommand>
{
    public async Task<Result> Handle(
        UpdateStartingBalanceCommand command, 
        CancellationToken cancellationToken)
    {
        Account? account = await context.Accounts
            .SingleOrDefaultAsync(
                a => a.AccountId == command.AccountId, 
                cancellationToken);

        if (account is null)
            {return Result.Failure(
                AccountErrors.NotFound(command.AccountId));}

        account.UpdateStartingBalance(
            command.UpdateStartingBalance,
            dateTimeProvider.UtcNowDateOnly);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
