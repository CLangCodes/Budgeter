using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Domain.Users;
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
    public async Task<Result<Guid>> Handle(
        RegisterUserCommand command, 
        CancellationToken cancellationToken)
    {
        var result = await identityService.RegisterUser(
            command.Email, 
            command.Password, 
            command.EnableNotifications,
            cancellationToken);

        
        if (!result.IsSuccess)
        { return Result.Failure<Guid>(UserErrors.RegistrationFailed); }
        return result.Value;
    }
}
