using Microsoft.AspNetCore.Identity;
using SharedKernel;
namespace Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public bool EnableNotifications { get; set; }
}