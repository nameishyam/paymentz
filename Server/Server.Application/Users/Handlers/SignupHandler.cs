using MediatR;
using Server.Application.Dto.Response;
using Server.Application.Exceptions;
using Server.Application.Interfaces.Repository;
using Server.Application.Interfaces.Service;
using Server.Application.Users.Commands;
using Server.Domain.Entities;

namespace Server.Application.Users.Handlers;

public class SignupHandler(
    IUserRepository userRepository,
    IEmailService emailService,
    IJwtService jwtService)
    : IRequestHandler<SignupCommand, SignupResponse>
{
    public async Task<SignupResponse> Handle(
        SignupCommand request,
        CancellationToken cancellationToken)
    {
        if (await userRepository.ExistsByEmail(request.Request.Email))
        {
            throw new ConflictException(request.Request.Email);
        }

        var user = new User
        {
            FirstName = request.Request.FirstName,
            LastName = request.Request.LastName,
            Email = request.Request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Request.Password)
        };

        await userRepository.Create(user);

        await emailService.SendEmailAsync(
            user.Email,
            "Welcome to PaymentZ",
            $"Glad to have you on board {user.FirstName} {user.LastName}");

        return new SignupResponse
        {
            Id = user.Id,
            AccessToken = jwtService.GenerateToken(user.Id, request.Request.Email)
        };
    }
}