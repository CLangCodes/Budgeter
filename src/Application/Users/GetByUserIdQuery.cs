using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetById;
public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;

internal sealed class GetUserByIdQueryHandler(
    IIdentityService identityService)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await identityService.GetUserById(query.UserId.ToString());

        if (user is null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFoundById);
        }
        
        return user;
    }
}