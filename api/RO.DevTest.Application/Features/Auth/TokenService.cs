using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RO.DevTest.Application.Contracts.Infrastructure;

namespace RO.DevTest.Application.Features.Auth;

/// <summary>
/// Service for generating JWT tokens.
/// </summary>
public class TokenService(IConfiguration configuration) : ITokenService
{
    /// <summary>
    /// Generates a JSON Web Token (JWT) for a given user with specified roles.
    /// </summary>
    /// <param name="user">The user for whom the access token is generated.</param>
    /// <param name="roles">A list of roles associated with the user.</param>
    /// <returns>A string representing the generated JWT access token.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the JWT key is not found in the configuration.</exception
    public string GenerateAccessToken(Domain.Entities.User user,
        IList<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found in configuration")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(ClaimTypes.Name, user.Name),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        // Add roles as claims
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// Generates a secure, base64-encoded refresh token that is URL-safe.
    /// </summary>
    /// <returns>A URL-safe, base64-encoded string representing the refresh token.</returns>
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        
        return Convert.ToBase64String(randomNumber)
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');
    }
}
