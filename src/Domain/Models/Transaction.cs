using SharedKernel;

namespace Domain.Models;

public enum TransactionType { Income = 0, Expense = 1 }

/// <summary>
/// In the end, it's all just money moving
/// </summary>
public class Transaction : Entity
{
    public int Id { get; private set; }
    public Guid? AccountId { get; private set; }
    public TransactionType TransactionType { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public DateOnly Date { get; private set; }
    public Guid? CategoryId { get; private set; }
    public Category? Category { get; private set; }

    public static Transaction Create(
        TransactionType transactionType,
        Guid accountId,
        string description,
        decimal amount,
        DateOnly date,
        Guid? categoryId)
    {
        return new Transaction
        {
            TransactionType = transactionType,
            AccountId = accountId,
            Description = description,
            Amount = amount,
            Date = date,
            CategoryId = categoryId,
        };
    }

    public void Update(
        TransactionType transactionType,
        Guid accountId,
        string description,
        decimal amount,
        DateOnly date,
        Guid? categoryId)
    {
        TransactionType = transactionType;
        AccountId = accountId;
        Description = description;
        Amount = amount;
        Date = date;
        CategoryId = categoryId;
    }
}

public static class TransactionErrors
{
    public static Error NotFound(int transactionId) => Error.NotFound(
        "Transactions.NotFound",
        $"The transaction with the Id = '{transactionId}' was not found");
}
