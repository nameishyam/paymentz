using MediatR;
using Server.Application.Interfaces.Repository;
using Server.Application.Users.Commands.Signup;
using Server.Domain.Entities;

namespace Server.Application.Users.Handlers;

public class SignupHandler(IUserRepository userRepository)
    : IRequestHandler<SignupCommand, Guid>
{
    public async Task<Guid> Handle(SignupCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            FirstName = request.Request.FirstName,
            LastName = request.Request.LastName,
            Email = request.Request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Request.Password)
        };

        await userRepository.Signup(user);

        return user.Id;
    }
}