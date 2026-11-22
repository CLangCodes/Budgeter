using SharedKernel;

namespace Domain.Users;

public sealed class User : Entity
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public bool EmailVerified { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PasswordHash { get; set; }
}
public static class UserErrors
{
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
}
