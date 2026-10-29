using SharedKernel;

namespace Domain.Models;

/// <summary>
/// In the end, it's all just money moving
/// </summary>
public class Transaction : Entity
{
    public int Id { get; set; }
    public int TransactionTypeId { get; set; } // 0 for Income, 1 for Expense
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Category { get; set; } = string.Empty;
    
    public static Transaction Create(
        int id,
        int transactionTypeId,
        string description,
        decimal amount,
        DateTime date,
        string category
    )
        {
            return new Transaction
            {
                Id = id,
                TransactionTypeId = transactionTypeId,
                Description = description,
                Amount = amount,
                Date = date,
                Category = category
            };
        }

    public void Update(
        int id,
        int transactionTypeId,
        string description,
        decimal amount,
        DateTime date,
        string category)
    {
        Id = id;
        TransactionTypeId = transactionTypeId;
        Description = description;
        Amount = amount;
        Date = date;
        Category = category;
    }
}