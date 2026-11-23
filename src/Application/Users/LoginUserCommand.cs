using System.Diagnostics;
using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.Login;

public sealed record LoginUserCommand(string Email, string Password, bool RememberMe) : ICommand;

internal sealed class LoginUserCommandHandler(
    IIdentityService identityService) : ICommandHandler<LoginUserCommand>
{
    public async Task<Result> Handle(LoginUserCommand command, CancellationToken cancellationToken)
    {
        var result = await identityService.LoginUser(
            command.Email, 
            command.Password, 
            command.RememberMe);
        
        return result;
    }
}
