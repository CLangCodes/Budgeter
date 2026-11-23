using Application.Abstractions.Messaging;
using Application.Users.Login;
using SharedKernel;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;

namespace BudgetBuddy.Endpoints.Users;

internal sealed class Login : IEndpoint
{
    public sealed record Request(
        string Email, 
        string Password, 
        bool RememberMe);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/login", async (
            Request request,
            ICommandHandler<LoginUserCommand> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new LoginUserCommand(
                request.Email, 
                request.Password, 
                request.RememberMe);

            Result result = await handler.Handle(command, cancellationToken);
            
            return result.Match(Results.SignIn, CustomResults.Problem);
        })
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithTags(Tags.Users);
    }
}
