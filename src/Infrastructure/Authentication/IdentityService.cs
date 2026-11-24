using Application.Abstractions.Authentication;
using Application.Users;
using Domain.Users;
using Infrastructure.Authorization;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Infrastructure.Authentication;

internal sealed class IdentityService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    TokenProvider tokenProvider,
    PermissionProvider permissionProvider) : IIdentityService
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
        var permissions = await permissionProvider.GetForUserIdAsync(user.Id);
        return tokenProvider.Create(user, roles, permissions);
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
        {
            bool duplicate = registrationResult.Errors.Any(e =>
                e.Code is nameof(IdentityErrorDescriber.DuplicateEmail)
                       or nameof(IdentityErrorDescriber.DuplicateUserName));

            return Result.Failure<Guid>(duplicate
                ? UserErrors.EmailNotUnique
                : UserErrors.RegistrationFailed);
        }

        var roleAssignResult = await userManager.AddToRoleAsync(user, Roles.Member);
        return roleAssignResult.Succeeded ?
            user.Id :
            Result.Failure<Guid>(UserErrors.RoleAssignmentFailed);
    }

    public async Task<Result<UserResponse>> GetUserByEmail(string email)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user is not null
            ? new UserResponse()
            {
                Id = user.Id,
                Email = user.Email!
            }
            : Result.Failure<UserResponse>(UserErrors.NotFoundByEmail);
        
    }
    
    public async Task<Result<UserResponse>> GetUserById(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user is not null
            ? new UserResponse()
            {
                Id = user.Id,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber ?? string.Empty,
                EnableNotifications = user.EnableNotifications
            }
            : Result.Failure<UserResponse>(UserErrors.NotFoundById);
    }

    public async Task<Result<UserResponse>> UpdateUser(
        string userId, 
        string phoneNumber, 
        bool enableNotifications)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
            return Result.Failure<UserResponse>(UserErrors.NotFoundById);

        if (user.PhoneNumber != phoneNumber)
            await userManager.SetPhoneNumberAsync(user, phoneNumber);

        if (user.EnableNotifications != enableNotifications)
            user.EnableNotifications = enableNotifications;
            
        await userManager.UpdateAsync(user);

        return new UserResponse()
        {
            Id = user.Id,
            PhoneNumber = phoneNumber,
            EnableNotifications = enableNotifications
        };
    }

}
