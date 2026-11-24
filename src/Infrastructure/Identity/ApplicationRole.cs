using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public sealed class ApplicationRole : IdentityRole<Guid>
{
    public ICollection<IdentityRoleClaim<Guid>> Claims { get; set; } = [];
}
