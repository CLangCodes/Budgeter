using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Accounts;

public sealed record GetAccountsQuery() : IQuery<GetAccountsResult>;
public sealed record AccountDetail(
    Guid AccountId,
    string NickName,
    AccountType AccountType,
    decimal StartingBalance,
    DateOnly CreatedAt,
    DateOnly UpdatedAt
);
public sealed record GetAccountsResult(List<AccountDetail> Accounts);

internal sealed class GetAccountsQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetAccountsQuery, GetAccountsResult>
{
    public async Task<Result<GetAccountsResult>> Handle(GetAccountsQuery query, CancellationToken cancellationToken)
    {
        var accounts = await context.Accounts
            .AsNoTracking()
            .Select(a => new AccountDetail(
                a.AccountId,
                a.NickName,
                a.AccountType,
                a.StartingBalance,
                a.CreatedAt,
                a.UpdatedAt))
            .ToListAsync(cancellationToken);

        return new GetAccountsResult(accounts);
    }
}
