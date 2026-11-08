using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration;

internal sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.AccountId);

        builder.Property(a => a.AccountType)
            .HasConversion<string>();

        // SQLite stores decimal as TEXT; store as REAL so it can be sorted/aggregated.
        builder.Property(a => a.StartingBalance)
            .HasConversion<double>();

        builder.HasMany(ac => ac.Transactions)
            .WithOne()
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
