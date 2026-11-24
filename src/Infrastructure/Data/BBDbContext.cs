using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Application.Abstractions.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Identity;
using Infrastructure.Authentication;

namespace Infrastructure.Data;

public class BBDbContext(DbContextOptions<BBDbContext> options) 
    : IdentityDbContext<
          ApplicationUser, 
          ApplicationRole, 
          Guid,
          IdentityUserClaim<Guid>,
          ApplicationUserRole,
          IdentityUserLogin<Guid>,
          IdentityRoleClaim<Guid>,
          IdentityUserToken<Guid>>(options), 
        IApplicationDbContext
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<BudgetItem> BudgetItems => Set<BudgetItem>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema(Schemas.Default);

        builder.ApplyConfigurationsFromAssembly(typeof(BBDbContext).Assembly);

        var identityAssembly = typeof(IdentityUser).Assembly; // Microsoft.Extensions.Identity.Stores
        foreach (var entityType in builder.Model.GetEntityTypes())
        {
            if (entityType.BaseType is not null || entityType.IsOwned()) continue; // root tables only

            var type = entityType.ClrType;
            while (type is not null && type.Assembly != identityAssembly)
                type = type.BaseType;

            if (type is not null)
                entityType.SetSchema(Schemas.Identity);
        }
    }
}