using FluentValidation;
using Server.Application.Users.Commands;

namespace Server.Application.Users.Validators;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.Request.AccessToken)
            .NotEmpty();

        RuleFor(x => x.Request.RefreshToken)
            .NotEmpty();
    }
}