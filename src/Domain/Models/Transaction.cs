using SharedKernel;

namespace Domain.Models;

/// <summary>
/// In the end, it's all just money moving
/// </summary>
public class Transaction : Entity
{
    public int Id { get; private set; }
    public Guid? AccountId { get; set; }
    public int TransactionTypeId { get; set; } // 0 for Income, 1 for Expense
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public Guid? CategoryId { get; set; }
    
    public static Transaction Create(
        int transactionTypeId,
        Guid accountId,
        string description,
        decimal amount,
        DateTime date,
        Guid? categoryId
    )
        {
            return new Transaction
            {
                TransactionTypeId = transactionTypeId,
                AccountId = accountId,
                Description = description,
                Amount = amount,
                Date = date,
                CategoryId = categoryId,
            };
        }

    public void Update(
        int transactionTypeId,
        Guid accountId,
        string description,
        decimal amount,
        DateTime date,
        Guid? categoryId)
    {
        TransactionTypeId = transactionTypeId;
        AccountId = accountId;
        Description = description;
        Amount = amount;
        Date = date;
        CategoryId = categoryId;
    }
}
internal sealed class TransactionsNotFoundException : Exception
{
    public TransactionsNotFoundException() : base () { }
    public TransactionsNotFoundException(string message) : base(message) { }
    public TransactionsNotFoundException(string message, Exception innerException) : base(message, innerException) { }
}
public static class TransactionErrors
{
    public static Error NotFound(int transactionId) => Error.NotFound(
        "Transactions.NotFound",
        $"The transaction with the Id = '{transactionId}' was not found");
}
