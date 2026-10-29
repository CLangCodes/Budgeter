using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Transactions;

public sealed record GetTransactionByIdQuery(int TransactionId) : IQuery<GetTransactionResponse>;
public sealed record GetTransactionResponse(
    int TransactionId, 
    int TransactionTypeId,
    string Description,
    decimal Amount,
    DateTime Date,
    string Category);

internal sealed class GetTransactionByIdQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetTransactionByIdQuery, GetTransactionResponse>
{
    public async Task<Result<GetTransactionResponse>> Handle(GetTransactionByIdQuery query, CancellationToken cancellationToken)
    {
        GetTransactionResponse? transaction = await context.Transactions
            .Where(transaction => transaction.Id == query.TransactionId)
            .Select(transaction => new GetTransactionResponse(
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
            return Result.Failure<GetTransactionResponse>(TransactionErrors.NotFound(query.TransactionId));
        }
        return transaction;
    }
}