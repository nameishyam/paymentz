using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Server.Application.Interfaces.Service;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Server.Application.Configurations;

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
}