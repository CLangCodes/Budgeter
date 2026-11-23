namespace BudgetBuddy.Endpoints.Users;

internal static class Permissions
{
    internal const string UsersAccess = "users:access";
    internal const string UsersRead = "user:read";
    internal const string UsersUpdate = "user:update";
    internal const string UsersDelete = "user:delete";
}

public static class CustomClaimTypes
{
    public const string Permission = "permission";
}