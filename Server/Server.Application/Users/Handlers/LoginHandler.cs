using MediatR;
using Server.Application.Dto.Response;
using Server.Application.Exceptions;
using Server.Application.Interfaces.Repository;
using Server.Application.Interfaces.Service;
using Server.Application.Users.Commands;

namespace Server.Application.Users.Handlers;

public class LoginHandler(
    IUserRepository userRepository,
    IJwtService jwtService)
    : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        if (!await userRepository.ExistsByEmail(request.Request.Email))
        {
            throw new NotFoundException(request.Request.Email);
        }

        var user = await userRepository.GetByEmail(request.Request.Email);

        if (!BCrypt.Net.BCrypt.Verify(request.Request.Password, user.Password))
        {
            throw new InvalidDetailsException();
        }

        return new LoginResponse
        {
            Id = user.Id,
            AccessToken = jwtService.GenerateToken(user.Id, user.Email)
        };
    }
}