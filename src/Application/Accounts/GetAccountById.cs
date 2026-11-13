using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Accounts;

public sealed record GetAccountByIdQuery(Guid AccountId) : IQuery<GetAccountResult>;
public sealed record GetAccountResult(
    Guid AccountId,
    string NickName,
    AccountType AccountType,
    decimal StartingBalance,
    DateOnly CreatedAt,
    DateOnly UpdatedAt);

internal sealed class GetAccountByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetAccountByIdQuery, GetAccountResult>
{
    public async Task<Result<GetAccountResult>> Handle(GetAccountByIdQuery query, CancellationToken cancellationToken)
    {
        GetAccountResult? account = await context.Accounts
            .Where(a => a.AccountId == query.AccountId)
            .Select(a => new GetAccountResult(
                a.AccountId,
                a.NickName,
                a.AccountType,
                a.StartingBalance,
                a.CreatedAt,
                a.UpdatedAt
            ))
            .SingleOrDefaultAsync(cancellationToken);
        if (account is null)
        {
            return Result.Failure<GetAccountResult>(AccountErrors.NotFound(query.AccountId));
        }
        return account;
    }
}
