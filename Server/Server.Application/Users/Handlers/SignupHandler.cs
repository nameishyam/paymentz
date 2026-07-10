using MediatR;
using Server.Application.Dto.Response;
using Server.Application.Interfaces.Repository;
using Server.Application.Interfaces.Service;
using Server.Application.Users.Commands;
using Server.Domain.Entities;

namespace Server.Application.Users.Handlers;

public class SignupHandler(
    IUserRepository userRepository,
    IJwtService jwtService)
    : IRequestHandler<SignupCommand, SignupResponse>
{
    public async Task<SignupResponse> Handle(SignupCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            FirstName = request.Request.FirstName,
            LastName = request.Request.LastName,
            Email = request.Request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Request.Password)
        };

        await userRepository.Signup(user);

        return new SignupResponse
        {
            Id = user.Id,
            AccessToken = jwtService.GenerateToken(user.Id, request.Request.Email)
        };
    }
}