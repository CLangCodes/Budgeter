namespace Application.Abstractions.Authorization;

/// <summary>
/// The single source of truth for permission names. Endpoints require them,
/// Infrastructure seeds them onto roles and enforces them.
/// </summary>
public static class Permissions
{
    public const string UsersRead = "user:read";
    public const string UsersUpdate = "user:update";
    public const string UsersDelete = "user:delete";

    public const string AccountsRead = "account:read";
    public const string AccountsWrite = "account:write";

    public const string TransactionsRead = "transaction:read";
    public const string TransactionsWrite = "transaction:write";

    public const string BudgetsRead = "budget:read";
    public const string BudgetsWrite = "budget:write";

    public const string CategoriesRead = "category:read";
    public const string CategoriesWrite = "category:write";

    public const string AnalysisRead = "analysis:read";
}
