using MediatR;
using Server.Application.Dto.Response;
using Server.Application.Interfaces.Repository;
using Server.Application.Interfaces.Service;
using Server.Application.Users.Commands;
using System.Security.Claims;

namespace Server.Application.Users.Handlers;

public class RefreshTokenHandler(
    IUserRepository userRepository,
    IJwtService jwtService)
    : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = jwtService.GetPrincipalFromExpiredToken(request.Request.AccessToken)
            ?? throw new UnauthorizedAccessException("Invalid access token structure.");

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID claim not found or invalid.");
        }

        var user = await userRepository.GetById(userId)
                   ?? throw new UnauthorizedAccessException("User not found.");

        if (user.RefreshToken != request.Request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");
        }

        var newAccessToken = jwtService.GenerateToken(user.Id, user.Email);
        var newRefreshToken = jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        await userRepository.Update(user);

        return new RefreshTokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken
        };
    }
}