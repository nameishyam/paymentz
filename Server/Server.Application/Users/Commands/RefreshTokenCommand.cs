using MediatR;
using Server.Application.Dto.Request;
using Server.Application.Dto.Response;

namespace Server.Application.Users.Commands;

public record RefreshTokenCommand(RefreshTokenRequest Request)
    : IRequest<RefreshTokenResponse>;