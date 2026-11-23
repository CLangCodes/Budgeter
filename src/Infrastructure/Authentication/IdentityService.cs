using Application.Abstractions.Authentication;
using Domain.Users;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using SharedKernel;

namespace Infrastructure.Authentication;

public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
{
    public async Task<Result<Guid>> RegisterUser(
        string email, 
        string password, 
        bool enableNotifications,
        CancellationToken cancellationToken)
    {
        var user = new ApplicationUser()
        {
            UserName = email,
            Email = email,
            EnableNotifications = enableNotifications
        };
        var result = await userManager.CreateAsync(user, password);
        return result.Succeeded
            ? user.Id : Result.Failure<Guid>(UserErrors.RegistrationFailed);
    }
}