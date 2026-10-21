using BudgetBuddy.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetBuddy.Kernel.Data;

public class BBDbContext(DbContextOptions<BBDbContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions => Set<Transaction>();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
        });
        base.OnModelCreating(builder);
    }
}