using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Account> Accounts { get; }
    DbSet<BudgetItem> BudgetItems { get; }
    DbSet<Category> Categories { get; }
    DbSet<Transaction> Transactions { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}