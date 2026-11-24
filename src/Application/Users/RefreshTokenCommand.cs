using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using SharedKernel;

namespace Application.Users.Login;

public sealed record RefreshTokenCommand(string RefreshToken) : ICommand<TokenResponse>;

internal sealed class RefreshTokenCommandHandler(
    IIdentityService identityService) : ICommandHandler<RefreshTokenCommand, TokenResponse>
{
    public async Task<Result<TokenResponse>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken) =>
        await identityService.RefreshToken(command.RefreshToken);
}
