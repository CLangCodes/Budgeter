using Domain.Models;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Transaction> Transactions { get; }
    DbSet<Category> Categories { get; }
    DbSet<BudgetItem> BudgetItems { get; }
    DbSet<Account> Accounts { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
