using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Transaction> Transactions { get; }
    DbSet<Category> Categories { get; }
    DbSet<Expense> Expenses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
