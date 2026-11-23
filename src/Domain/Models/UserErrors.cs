using SharedKernel;

namespace Domain.Users;

public static class UserErrors
{
    public static readonly Error LoginFailed = Error.Unauthorized(
        "Users.LoginFailed",
        "Invalid email or password.");

    public static readonly Error LockedOut = Error.Unauthorized(
        "Users.LockedOut",
        "Account is temporarily locked. Try again later.");

    public static Error NotFound(Guid userId) => Error.NotFound(
        "Users.NotFound",
        $"The user with the Id = '{userId}' was not found");

    public static Error Unauthorized() => Error.Failure(
        "Users.Unauthorized",
        "You are not authorized to perform this action.");

    public static readonly Error NotFoundByEmail = Error.NotFound(
        "Users.NotFoundByEmail",
        "The user with the specified email was not found");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Users.EmailNotUnique",
        "The provided email is not unique");

    public static Error RegistrationFailed => Error.Failure(
        "Users.RegistrationFailed",
        "The registration of the user failed");

    public static Error RoleAssignmentFailed => Error.Failure(
        "Users.RoleAssignmentFailed",
        "The user's role assignment failed");
}
