using MediatR;
using Server.Application.Dto.Request;
using Server.Application.Dto.Response;

namespace Server.Application.Users.Commands;

public record LoginCommand(LoginRequest Request)
    : IRequest<LoginResponse>;