using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Update;

public sealed record UpdateUserCommand(
    string UserId, 
    string PhoneNumber, 
    bool EnableNotifications) : ICommand<UserResponse>;

internal sealed class UpdateUserCommandHandler(
    IIdentityService identityService) : ICommandHandler<UpdateUserCommand, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(
        UpdateUserCommand command, 
        CancellationToken cancellationToken) =>
        await identityService.UpdateUser(
            command.UserId,
            command.PhoneNumber,
            command.EnableNotifications);
}
