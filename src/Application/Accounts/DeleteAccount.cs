using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Accounts;

public sealed record DeleteAccountCommand(Guid AccountId) : ICommand;

internal sealed class DeleteAccountCommandHandler(IApplicationDbContext context)
    : ICommandHandler<DeleteAccountCommand>
{
    public async Task<Result> Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
    {
        Account? account = await context.Accounts.SingleOrDefaultAsync(
            a => a.AccountId == command.AccountId, cancellationToken);

        if (account is null)
            {return Result.Failure(AccountErrors.NotFound(command.AccountId));}
        context.Accounts.Remove(account);

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
