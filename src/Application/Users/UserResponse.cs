namespace Application.Users;

public sealed record UserResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public bool EnableNotifications { get; init; }
}
