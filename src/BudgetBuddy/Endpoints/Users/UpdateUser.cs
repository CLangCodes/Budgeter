using Application.Abstractions.Messaging;
using Application.Users.GetById;
using SharedKernel;
using BudgetBuddy.Extensions;
using BudgetBuddy.Infrastructure;
using Infrastructure.Identity;
using Domain.Users;
using Application.Users.Login;
using Microsoft.AspNetCore.Mvc;

namespace BudgetBuddy.Endpoints.Users;

internal sealed class UpdateUser : IEndpoint
{
    internal sealed record Request(string UserId, string PhoneNumber, bool EnableNotification);
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("users/{userId:guid}", async (
            string userId,
            [FromBody]
            Request request,
            ICommandHandler<UpdateUserCommand, UserResponse> handler,
            CancellationToken cancellationToken) =>
        {
            Result<UserResponse> result = await handler.Handle(
                new UpdateUserCommand(
                    userId,
                    request.PhoneNumber, 
                    request.EnableNotification
                ), cancellationToken);

            return result.Match(Results.Ok, CustomResults.Problem);
        })
        .HasPermission(Permissions.UsersUpdate)
        .Produces<UserResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status401Unauthorized)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithTags(Tags.Users);
    }
}
