using Microsoft.AspNetCore.Identity;
using SharedKernel;
namespace Infrastructure.Identity;

public sealed class ApplicationUserRole : IdentityUserRole<Guid>
{
    public ApplicationRole Role { get; set; }
}