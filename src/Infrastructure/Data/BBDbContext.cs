using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Application.Abstractions.Data;

namespace Infrastructure.Data;

public class BBDbContext(DbContextOptions<BBDbContext> options) 
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<BudgetItem> BudgetItems => Set<BudgetItem>();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(BBDbContext).Assembly);

        builder.Entity<Transaction>(entity =>
        {
            entity.HasKey(t => t.Id);
        });
        base.OnModelCreating(builder);
    }
}