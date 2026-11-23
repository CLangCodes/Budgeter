using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Login;

public sealed record LoginUserCommand(string Email, string Password, bool RememberMe) : ICommand<string>;

internal sealed class LoginUserCommandHandler(
    IIdentityService identityService) : ICommandHandler<LoginUserCommand, string>
{
    public async Task<Result<string>> Handle(LoginUserCommand command, CancellationToken cancellationToken) =>
        await identityService.LoginUser(
            command.Email,
            command.Password,
            command.RememberMe);
}
