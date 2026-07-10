using FluentValidation;
using Server.Application.Users.Commands;

namespace Server.Application.Users.Validators;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Request.Password)
            .NotEmpty();
    }
}