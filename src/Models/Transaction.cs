namespace BudgetBuddy.Models
{
    /// <summary>
    /// In the end, it's all just money moving
    /// </summary>
    public class Transaction
    {
        public int Id { get; set; }
        public int TransactionTypeId { get; set; } // 0 for Income, 1 for Expense
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; } = string.Empty;
    }
}