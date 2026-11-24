using Application.Users;
using SharedKernel;

namespace Application.Abstractions.Authentication;

public interface IIdentityService
{
    public Task<Result<Guid>> RegisterUser(
        string email, 
        string password, 
        bool enableNotifications);
    public Task<Result<TokenResponse>> LoginUser(
        string email,
        string password,
        bool rememberMe
    );
    public Task<Result<TokenResponse>> LoginWithRefreshTokenCommand(
        string RefreshToken
    );
    public Task<Result<UserResponse>> GetUserByEmail(
        string email
    );
    public Task<Result<UserResponse>> GetUserById(
        string userId
    );
    public Task<Result<UserResponse>> UpdateUser(
        string UserId, 
        string PhoneNumber, 
        bool EnableNotifications
    );
    public Task<Result> RevokeRefreshTokens(Guid userId);
}