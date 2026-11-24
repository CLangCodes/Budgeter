using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;

namespace Infrastructure.Authentication;

internal sealed class TokenProvider(
    IConfiguration configuration)
{
    public string Create(
        ApplicationUser user,
        IEnumerable<string> roles,
        IEnumerable<string> permissions)
    {
        string secretKey = configuration["Jwt:Secret"]
            ?? throw new InvalidOperationException(
                "JWT secret is not configured.");

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(
            securityKey, 
            SecurityAlgorithms.HmacSha256);

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub,
                user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email,
                user.Email ?? string.Empty),
            new("email_verified",
                user.EmailConfirmed
                .ToString()
                .ToLowerInvariant()),
            ..roles.Select(
                role => new Claim(
                    ClaimTypes.Role, 
                    role)),
            ..permissions.Select(
                permission => new Claim(
                    CustomClaimTypes.Permission, 
                    permission))
        ];

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(
                configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            SigningCredentials = credentials,
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"]
        };

        var tokenHandler = new JsonWebTokenHandler();

        return tokenHandler.CreateToken(tokenDescriptor);
    }

    public string GenerateRefreshToken(
        IDateTimeProvider dateTimeProvider,
        ApplicationUser user,
        string refreshToken)
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }
    }
