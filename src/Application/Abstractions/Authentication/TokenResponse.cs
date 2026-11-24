using Application.Abstractions.Authentication;

namespace Application.Abstractions.Authentication;

public sealed record TokenResponse(string AccessToken, string RefreshToken);
