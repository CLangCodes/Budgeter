using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Login;

public sealed record LoginWithRefreshTokenCommand(string RefreshToken) 
    : ICommand<TokenResponse>;

internal sealed class RefreshTokenCommandHandler(
    IIdentityService identityService) 
    : ICommandHandler<LoginWithRefreshTokenCommand, TokenResponse>
{
    public async Task<Result<TokenResponse>> Handle(
        LoginWithRefreshTokenCommand command, 
        CancellationToken cancellationToken) =>
            await identityService.LoginWithRefreshTokenCommand(
                command.RefreshToken);
}
