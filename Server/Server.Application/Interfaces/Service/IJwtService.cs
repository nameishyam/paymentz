using System.Security.Claims;

namespace Server.Application.Interfaces.Service;

public interface IJwtService
{
    string GenerateToken(Guid userId, string email);
    string GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}