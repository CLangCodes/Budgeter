using Domain.Models;
namespace Application.Transactions;

public sealed record TransactionDetail(
    int Id, 
    Guid AccountId, 
    TransactionType TransactionType, 
    string Description, 
    decimal Amount, 
    DateOnly Date, 
    Guid? CategoryId,
    string CategoryName,
    string CategoryDescription);
