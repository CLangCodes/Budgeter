using System.Security.Claims;
using BudgetBuddy.Endpoints.Users;
using Infrastructure.Authentication;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace BudgetBuddy.Extensions;
public static class IdentityExtensions
{
    public static async Task SeedIdentityRoles(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        var adminRole = await roleManager.FindByNameAsync(Roles.Admin);
        if (adminRole is null)
        {
            await roleManager.CreateAsync(adminRole = new IdentityRole<Guid>(Roles.Admin));
            await roleManager.AddClaimAsync(
                adminRole, 
                new Claim(CustomClaimTypes.Permission, Permissions.UsersRead));
            await roleManager.AddClaimAsync(
                adminRole, 
                new Claim(CustomClaimTypes.Permission, Permissions.UsersUpdate));
            await roleManager.AddClaimAsync(
                adminRole, 
                new Claim(CustomClaimTypes.Permission, Permissions.UsersDelete));
        }
        var memberRole = await roleManager.FindByNameAsync(Roles.Member);

        if (memberRole is null)
        {
            await roleManager.CreateAsync(memberRole = new IdentityRole<Guid>(Roles.Member));
            await roleManager.AddClaimAsync(
                memberRole, 
                new Claim(CustomClaimTypes.Permission, Permissions.UsersRead));
            await roleManager.AddClaimAsync(
                memberRole, 
                new Claim(CustomClaimTypes.Permission, Permissions.UsersUpdate));
        }
    }
}