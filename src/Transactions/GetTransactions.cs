using BudgetBuddy.Kernel.Messaging;

namespace BudgetBuddy.Transactions;

internal sealed record GetTransactionsQuery() : IQuery<GetTransactionResults>;
internal sealed record GetTransactionResults();
internal class GetTransactionQueryHandler()
{
    
}