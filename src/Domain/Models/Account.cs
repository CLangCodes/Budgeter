using SharedKernel;

namespace Domain.Models;

public enum AccountType { Cash, Checking, Savings }

public class Account : Entity
{
    public Guid AccountId { get; private set; }
    public List<Transaction> Transactions { get; private set; } = [];
    public string NickName { get; private set; } = string.Empty;
    public AccountType AccountType { get; private set; }
    public decimal StartingBalance { get; private set; }
    public DateOnly CreatedAt { get; private set; }
    public DateOnly UpdatedAt { get; private set; }
    public decimal Balance => StartingBalance - Transactions.Sum(t => t.Amount);
    public List<BudgetItem> BudgetItems { get; private set; } = [];

    public static Account Create(
        string nickName,
        AccountType accountType,
        decimal startingBalance,
        DateOnly utcNow)
    {
        return new Account
        {
            AccountId = Guid.NewGuid(),
            NickName = nickName,
            AccountType = accountType,
            StartingBalance = startingBalance,
            CreatedAt = utcNow,
            UpdatedAt = utcNow
        };
    }

    public void Update(
        string nickName,
        AccountType accountType,
        decimal startingBalance,
        DateOnly utcNow)
    {
        NickName = nickName;
        AccountType = accountType;
        StartingBalance = startingBalance;
        UpdatedAt = utcNow;
    }

    public void UpdateStartingBalance(decimal updatedStartingBalance, DateOnly utcNow)
    {
        StartingBalance = updatedStartingBalance;
        UpdatedAt = utcNow;
    }
}

public static class AccountErrors
{
    public static Error NotFound(Guid accountId) => Error.NotFound(
        "Accounts.NotFound",
        $"The account with the Id = '{accountId}' was not found");
}
