using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal sealed class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(e => e.Id);

        // Stored as its integer value in the original column; no data migration needed.
        builder.Property(e => e.TransactionType)
            .HasColumnName("TransactionTypeId");

        builder.HasOne(tr => tr.Category)
            .WithMany()
            .HasForeignKey(e => e.CategoryId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
