using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Accounts;

public sealed record GetAccountBalanceQuery(Guid AccountId) : IQuery<GetAccountBalanceResult>;
public sealed record GetAccountBalanceResult(
    decimal Balance,
    DateTime RetrievedAt);

internal sealed class GetAccountBalanceQueryHandler(IApplicationDbContext context, IDateTimeProvider dateTimeProvider)
    : IQueryHandler<GetAccountBalanceQuery, GetAccountBalanceResult>
{
    public async Task<Result<GetAccountBalanceResult>> Handle(
        GetAccountBalanceQuery query, 
        CancellationToken cancellationToken)
    {
        // Load the transactions so the domain's Account.Balance rule does the calculation.
        // (Projecting a.Balance in SQL silently drops the Include, and SQLite cannot Sum decimals.)
        Account? account = await context.Accounts
            .AsNoTracking()
            .Include(a => a.Transactions)
            .SingleOrDefaultAsync(a => a.AccountId == query.AccountId, cancellationToken);
        if (account is null)
        {
            return Result.Failure<GetAccountBalanceResult>(AccountErrors.NotFound(query.AccountId));
        }
        return new GetAccountBalanceResult(account.Balance, dateTimeProvider.UtcNow);
    }
}
