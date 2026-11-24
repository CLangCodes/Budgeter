using Application.Abstractions.Authorization;
using Application.Abstractions.Messaging;
using Application.Users;
using BudgetBuddy.Extensions;
using BudgetBuddy.Http;
using SharedKernel;

namespace BudgetBuddy.Endpoints.Users;

internal sealed class RevokeRefreshToken : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/users/revoke/{userId:Guid}", async (
        Guid userId,
        ICommandHandler<RevokeRefreshTokenCommand, bool> handler,
        CancellationToken cancellationToken) =>
        {
            Result result = await handler.Handle(
                new RevokeRefreshTokenCommand(userId),
                cancellationToken).ConfigureAwait(false);
            return result.Match(Results.NoContent, CustomResults.Problem);
        })
        .HasPermission(Permissions.UsersRead)
        .WithTags(Tags.Users)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound);
    }
}