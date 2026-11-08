using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Accounts;

public sealed record UpdateAccountBalanceCommand(
    Guid AccountId,
    decimal UpdatedAccountBalance
) : ICommand;

internal sealed class UpdateAccountBalanceCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider dateTimeProvider
) : ICommandHandler<UpdateAccountBalanceCommand>
{
    public async Task<Result> Handle(
        UpdateAccountBalanceCommand command, 
        CancellationToken cancellationToken)
    {
        Account? account = await context.Accounts
            .SingleOrDefaultAsync(
                a => a.AccountId == command.AccountId, 
                cancellationToken);

        if (account is null)
            {return Result.Failure(
                AccountErrors.NotFound(command.AccountId));}

        account.UpdateBalance(
            command.UpdatedAccountBalance,
            dateTimeProvider.UtcNow);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
