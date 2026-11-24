using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Users.Login;
using BudgetBuddy.Extensions;
using BudgetBuddy.Http;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Endpoints.Users;

internal sealed class LoginWithRefreshToken : IEndpoint
{
    public sealed record Request(string RefreshToken);
    public sealed record Response(TokenResponse TokenResponse);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/refresh", async (
            [FromBody]
            Request request,
            ICommandHandler<LoginWithRefreshTokenCommand, TokenResponse> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new LoginWithRefreshTokenCommand(request.RefreshToken);

            var result = await handler.Handle(command, cancellationToken);

            return result.Match(token => Results.Ok(new Response(token)), CustomResults.Problem);
        })
        .AllowAnonymous()
        .Produces<Response>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .WithTags(Tags.Users);
    }
}