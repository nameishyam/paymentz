using MediatR;

namespace Server.Application.Users.Commands.Signup;

public record SignupCommand(SignupRequest Request) 
    : IRequest<Guid>;