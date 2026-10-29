using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Transactions;

public sealed record GetTransactionByIdQuery(int TransactionId) : IQuery<GetTransactionResult>;
public sealed record GetTransactionResult(
    int TransactionId, 
    int TransactionTypeId,
    string Description,
    decimal Amount,
    DateTime Date,
    string Category);

internal sealed class GetTransactionByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetTransactionByIdQuery, GetTransactionResult>
{
    public async Task<Result<GetTransactionResult>> Handle(GetTransactionByIdQuery query, CancellationToken cancellationToken)
    {
        GetTransactionResult? transaction = await context.Transactions
            .Where(transaction => transaction.Id == query.TransactionId)
            .Select(transaction => new GetTransactionResult(
                transaction.Id,
                transaction.TransactionTypeId,
                transaction.Description,
                transaction.Amount,
                transaction.Date,
                transaction.Category
            ))
            .SingleOrDefaultAsync(cancellationToken);
        if (transaction is null)
        {
            return Result.Failure<GetTransactionResult>(TransactionErrors.NotFound(query.TransactionId));
        }
        return transaction;
    }
}