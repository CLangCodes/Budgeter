using Infrastructure.Authentication;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Authorization;

internal sealed class PermissionProvider(BBDbContext context)
{
    public async Task<HashSet<string>> GetForUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var permissions = await (
            from userRole in context.UserRoles
            join claim in context.RoleClaims on userRole.RoleId equals claim.RoleId
            where userRole.UserId == userId &&
                claim.ClaimType == CustomClaimTypes.Permission &&
                claim.ClaimValue != null
            select claim.ClaimValue!)
            .Distinct()
            .ToListAsync(cancellationToken);

        return [.. permissions];
    }
}
