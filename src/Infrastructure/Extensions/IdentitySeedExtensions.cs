using System.Security.Claims;
using Application.Abstractions.Authorization;
using Infrastructure.Authentication;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class IdentitySeedExtensions
{
    private static readonly string[] MemberPermissions =
    [
        Permissions.UsersRead,
        Permissions.UsersUpdate,
        Permissions.AccountsRead,
        Permissions.AccountsWrite,
        Permissions.TransactionsRead,
        Permissions.TransactionsWrite,
        Permissions.BudgetsRead,
        Permissions.BudgetsWrite,
        Permissions.CategoriesRead,
        Permissions.CategoriesWrite,
        Permissions.AnalysisRead,
    ];

    private static readonly Dictionary<string, string[]> RolePermissions = new()
    {
        [RoleTypes.Member] = MemberPermissions,
        [RoleTypes.Admin] = [.. MemberPermissions, Permissions.UsersDelete],
    };

    /// <summary>
    /// Ensures every role exists and carries every permission claim it is mapped to.
    /// Idempotent: safe to run on every startup, and adds newly introduced permissions
    /// to roles that already exist.
    /// </summary>
    public static async Task SeedIdentityRolesAsync(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        foreach (var (roleName, permissions) in RolePermissions)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role is null)
            {
                role = new ApplicationRole()
                {
                    Name = roleName
                };
                await roleManager.CreateAsync(role);
            }

            var existing = (await roleManager.GetClaimsAsync(role))
                .Where(c => c.Type == CustomClaimTypes.Permission)
                .Select(c => c.Value)
                .ToHashSet();

            foreach (var permission in permissions.Where(p => !existing.Contains(p)))
            {
                await roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, permission));
            }
        }
    }
}
