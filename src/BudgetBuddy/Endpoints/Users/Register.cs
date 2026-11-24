using Application.Abstractions.Messaging;
using Application.Users.Register;
using SharedKernel;
using BudgetBuddy.Extensions;
using BudgetBuddy.Http;

namespace BudgetBuddy.Endpoints.Users;

internal sealed class Register : IEndpoint
{
    public sealed record Request(string Email, string Password, bool EnableNotifications);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("users/register", async (
            Request request,
            ICommandHandler<RegisterUserCommand, Guid> handler,
            CancellationToken cancellationToken) =>
        {
            var command = new RegisterUserCommand(
                request.Email,
                request.Password,
                request.EnableNotifications);

            Result<Guid> result = await handler.Handle(command, cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .AllowAnonymous()
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .WithTags(Tags.Users);
    }
}
