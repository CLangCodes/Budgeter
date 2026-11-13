namespace Application.Transactions;

public sealed record TransactionDetail(
    int Id, 
    Guid AccountId, 
    int TransactionTypeId, 
    string Description, 
    decimal Amount, 
    DateOnly Date, 
    Guid? CategoryId,
    string CategoryName,
    string CategoryDescription);
