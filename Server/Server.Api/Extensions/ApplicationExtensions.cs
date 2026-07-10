using FluentValidation;
using MediatR;
using Server.Application;
using Server.Application.Users.Commands.Signup;
using Server.Application.Users.Validators;

namespace Server.Api.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(SignupCommand).Assembly);
        });

        services.AddValidatorsFromAssembly(typeof(SignupValidator).Assembly);

        services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return services;
    }
}