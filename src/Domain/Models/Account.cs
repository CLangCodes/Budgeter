using SharedKernel;

namespace Domain.Models;

public enum AccountType { Cash, Checking, Savings }

public class Account : Entity
{
    public Guid AccountId { get; set; }
    public List<Transaction> Transactions { get; set; } = [];
    public string NickName { get; set; } = string.Empty;
    public AccountType AccountType { get; set; }
    public decimal StartingBalance { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public decimal Balance => StartingBalance - Transactions.Sum(t => t.Amount);

    public static Account Create(
        string nickName,
        AccountType accountType,
        decimal startingBalance,
        DateTime utcNow)
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
        DateTime utcNow)
    {
        NickName = nickName;
        AccountType = accountType;
        StartingBalance = startingBalance;
        UpdatedAt = utcNow;
    }

    public void UpdateStartingBalance(decimal updatedStartingBalance, DateTime utcNow)
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
