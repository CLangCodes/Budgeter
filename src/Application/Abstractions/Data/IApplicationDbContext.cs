using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Transaction> Transactions { get; }
    DbSet<Category> Categories { get; }
    DbSet<BudgetItem> BudgetItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
