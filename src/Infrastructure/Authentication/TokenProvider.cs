using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Infrastructure.Authorization;
using Infrastructure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using SharedKernel;

namespace Infrastructure.Authentication;

internal sealed class TokenProvider(
    IConfiguration configuration)
{
    public async Task<string> Create(
        ApplicationUser user)
    {
        string secretKey = configuration["Jwt:Secret"]
            ?? throw new InvalidOperationException(
                "JWT secret is not configured.");

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(
            securityKey, 
            SecurityAlgorithms.HmacSha256);
        IEnumerable<string> roles = [.. user.UserRoles
            .Select(ur => ur.Role)
                .Select(r => r.Name ?? string.Empty)];

        IEnumerable<string> permissions = [.. user.UserRoles
            .Select(ur => ur.Role)
                .SelectMany(r => r.Claims.Where(
                        c => c.ClaimType == CustomClaimTypes.Permission))
                    .Select(c => c.ClaimValue ?? string.Empty)];
        
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

    public string GenerateRefreshToken()
        => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
    }
