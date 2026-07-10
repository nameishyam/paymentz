namespace Server.Application.Interfaces.Service;

public interface IJwtService
{
    string GenerateToken(Guid userId, string email);
}