using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Server.Application.Configurations;
using Server.Application.Interfaces.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Server.Infrastructure.Services;

public class JwtService(IOptions<JwtConfigurations> jwtConfigurations) : IJwtService
{
    public string GenerateToken(Guid userId, string email)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtConfigurations.Value.SecretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtConfigurations.Value.Issuer,
            audience: jwtConfigurations.Value.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(jwtConfigurations.Value.ExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtConfigurations.Value.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtConfigurations.Value.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtConfigurations.Value.SecretKey)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var principal = tokenHandler.ValidateToken(
                token,
                tokenValidationParameters, 
                out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(
                    SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return principal;
        }
        catch
        {
            return null;
        }
    }
}