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
        GetAccountBalanceResult? account = await context.Accounts
            .AsNoTracking()
            .Where(a => a.AccountId == query.AccountId)
            .Include(account => account.Transactions)
            .Select(a => new GetAccountBalanceResult(
                a.Balance,
                dateTimeProvider.UtcNow
            ))
            .SingleOrDefaultAsync(cancellationToken);
        if (account is null)
        {
            return Result.Failure<GetAccountBalanceResult>(AccountErrors.NotFound(query.AccountId));
        }
        return account;
    }
}
