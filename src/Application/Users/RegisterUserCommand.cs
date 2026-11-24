using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Register;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    bool EnableNotifications)
    : ICommand<Guid>;

internal sealed class RegisterUserCommandHandler(
    IIdentityService identityService)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    // The identity service already returns specific errors
    // (EmailNotUnique, RegistrationFailed, RoleAssignmentFailed); pass them through.
    public async Task<Result<Guid>> Handle(
        RegisterUserCommand command,
        CancellationToken cancellationToken) =>
        await identityService.RegisterUser(
            command.Email,
            command.Password,
            command.EnableNotifications);
}
