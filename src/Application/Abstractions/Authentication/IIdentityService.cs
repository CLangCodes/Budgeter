using Application.Users.GetById;
using SharedKernel;

namespace Application.Abstractions.Authentication;

public interface IIdentityService
{
    public Task<Result<Guid>> RegisterUser(
        string email, 
        string password, 
        bool enableNotifications);
    public Task<Result<string>> LoginUser(
        string email,
        string password,
        bool rememberMe
    );
    public Task<Result<UserResponse>> GetUserByEmail(
        string email
    );
    public Task<Result<UserResponse>> GetUserById(
        string userId
    );
}