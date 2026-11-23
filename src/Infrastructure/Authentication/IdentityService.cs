using System.Reflection.Metadata.Ecma335;
using Application.Abstractions.Authentication;
using Domain.Users;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Infrastructure.Authentication;

public class IdentityService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager) : IIdentityService
{
    public async Task<Result> LoginUser(
        string email, 
        string password, 
        bool rememberMe)
    {
        var signInResult = await signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: true);
        return signInResult.Succeeded
        ? Result.Success()
        : Result.Failure(UserErrors.LoginFailed);
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