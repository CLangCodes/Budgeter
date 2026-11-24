using Application.Abstractions.Authentication;
using Application.Users;
using Domain.Users;
using Infrastructure.Authorization;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Time;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Infrastructure.Authentication;
internal sealed class IdentityService(
    BBDbContext context,
    IDateTimeProvider dateTimeProvider,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    TokenProvider tokenProvider) : IIdentityService
{
    public async Task<Result<TokenResponse>> LoginUser(
        string email,
        string password,
        bool rememberMe)
    {
        // rememberMe is intentionally unused until refresh tokens are implemented.
        var normalizedEmail = userManager.NormalizeEmail(email);

        var user = await userManager.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                    .ThenInclude(r => r.Claims)
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.NormalizedEmail == normalizedEmail);

        if (user is null)
            return Result.Failure<TokenResponse>(UserErrors.LoginFailed);

        // Verifies the password and tracks lockout without issuing a cookie.
        var check = await signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: true);
        if (check.IsLockedOut)
            return Result.Failure<TokenResponse>(UserErrors.LockedOut);
        if (!check.Succeeded)
            return Result.Failure<TokenResponse>(UserErrors.LoginFailed);

        var accessToken = await tokenProvider.Create(user);

        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = tokenProvider.GenerateRefreshToken(),
            ExpiresOnUtc = dateTimeProvider.UtcNow.AddDays(1) // ToDo: Configure this in ApplicationSettings
        };
        
        context.RefreshTokens.Add(refreshToken);
        
        await context.SaveChangesAsync();

        return new TokenResponse(accessToken, refreshToken.Token);
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

        var roleAssignResult = await userManager.AddToRoleAsync(user, RoleTypes.Member);
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

    public async Task<Result<TokenResponse>> LoginWithRefreshTokenCommand(string RefreshToken)
    {
        RefreshToken? refreshToken = await context.RefreshTokens
            .Include(r => r.User)
                .ThenInclude(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                        .ThenInclude(r => r.Claims)
            .FirstOrDefaultAsync(r => r.Token == RefreshToken);

        if (refreshToken is null
            || refreshToken.ExpiresOnUtc < dateTimeProvider.UtcNow)
            return Result.Failure<TokenResponse>(UserErrors.LoginFailed);
        
        string accessToken = await tokenProvider.Create(refreshToken.User);

        refreshToken.Token = tokenProvider.GenerateRefreshToken();
        refreshToken.ExpiresOnUtc = dateTimeProvider.UtcNow.AddDays(1); // ToDo: Configure in AppSettings

        await context.SaveChangesAsync();
        return new TokenResponse(accessToken, refreshToken.Token);
    }

    public async Task<Result> RevokeRefreshTokens(Guid userId)
    {
        await context.RefreshTokens
            .Where(r => r.UserId == userId)
            .ExecuteDeleteAsync();

        return Result.Success();
    }
}
