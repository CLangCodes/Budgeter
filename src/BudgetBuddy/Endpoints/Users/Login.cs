using Application.Abstractions.Messaging;
using Application.Users.Login;
using SharedKernel;
using BudgetBuddy.Extensions;
using BudgetBuddy.Http;
using Application.Abstractions.Authentication;

namespace BudgetBuddy.Endpoints.Users;

internal sealed class Login : IEndpoint
{
    public sealed record Request(
        string Email, 
        string Password, 
        bool RememberMe = false);
    public sealed record Response(
        TokenResponse TokenResponse);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/login", async (
            Request request,
            ICommandHandler<LoginUserCommand, TokenResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new LoginUserCommand(
                request.Email,
                request.Password,
                request.RememberMe);

            var result = await handler.Handle(command, cancellationToken);

            return result.Match(token => Results.Ok(new Response(token)), CustomResults.Problem);
        })
        .AllowAnonymous()
        .Produces<Response>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithTags(Tags.Users);
    }
}
