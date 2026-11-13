using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Transactions;

public sealed record GetTransactionByIdQuery(int TransactionId) : IQuery<GetTransactionResult>;
public sealed record GetTransactionResult(
    int TransactionId, 
    Guid AccountId,
    int TransactionTypeId,
    string Description,
    decimal Amount,
    DateOnly Date,
    Guid? CategoryId,
    string CategoryName,
    string CategoryDescription);

internal sealed class GetTransactionByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetTransactionByIdQuery, GetTransactionResult>
{
    public async Task<Result<GetTransactionResult>> Handle(GetTransactionByIdQuery query, CancellationToken cancellationToken)
    {
        GetTransactionResult? transaction = await context.Transactions
            .AsNoTracking()
            .Where(transaction => transaction.Id == query.TransactionId)
            .Include(transaction => transaction.Category)
            .Select(transaction => new GetTransactionResult(
                transaction.Id,
                transaction.AccountId ?? Guid.Empty,
                transaction.TransactionTypeId,
                transaction.Description,
                transaction.Amount,
                transaction.Date,
                transaction.CategoryId,
                transaction.Category!.CategoryName ?? string.Empty,
                transaction.Category.Description
            ))
            .SingleOrDefaultAsync(cancellationToken);
        if (transaction is null)
        {
            return Result.Failure<GetTransactionResult>(TransactionErrors.NotFound(query.TransactionId));
        }
        return transaction;
    }
}