using Application.Abstractions.Authentication;
using Domain.Users;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Infrastructure.Authentication;

internal sealed class IdentityService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    TokenProvider tokenProvider) : IIdentityService
{
    public async Task<Result<string>> LoginUser(
        string email,
        string password,
        bool rememberMe)
    {
        // rememberMe is intentionally unused until refresh tokens are implemented.
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result.Failure<string>(UserErrors.LoginFailed);

        // Verifies the password and tracks lockout without issuing a cookie.
        var check = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
        if (check.IsLockedOut)
            return Result.Failure<string>(UserErrors.LockedOut);
        if (!check.Succeeded)
            return Result.Failure<string>(UserErrors.LoginFailed);

        var roles = await userManager.GetRolesAsync(user);
        return tokenProvider.Create(user, roles);
    }

    public async Task<Result<Guid>> RegisterUser(
        string email,
        string password,
        bool enableNotifications)
    {
        var user = new ApplicationUser()
        {
            UserName = email,
            Email = email,
            EnableNotifications = enableNotifications
        };
        var registrationResult = await userManager.CreateAsync(user, password);
        if (!registrationResult.Succeeded)
            return Result.Failure<Guid>(UserErrors.RegistrationFailed);

        var roleAssignResult = await userManager.AddToRoleAsync(user, Roles.Member);
        return roleAssignResult.Succeeded ?
            user.Id :
            Result.Failure<Guid>(UserErrors.RoleAssignmentFailed);
    }
}
