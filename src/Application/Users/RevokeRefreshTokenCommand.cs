using System.Security.Claims;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users;
public sealed record RevokeRefreshTokenCommand(Guid UserId) : ICommand<bool>;
internal sealed class RevokeRefreshTokenCommandHandler(
    IIdentityService identityService,
    IUserContext userContext)
    : ICommandHandler<RevokeRefreshTokenCommand, bool>
{
    public async Task<Result<bool>> Handle(RevokeRefreshTokenCommand command, CancellationToken cancellationToken)
    {
        if (command.UserId != userContext.UserId)
            throw new ApplicationException("You can't do this");

        var result = await identityService.RevokeRefreshTokens(command.UserId);

        return result.IsSuccess
            ? true 
            : Result.Failure<bool>(result.Error);
    }
}