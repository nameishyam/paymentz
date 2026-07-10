using FluentValidation;
using Server.Application.Users.Commands;

namespace Server.Application.Users.Validators;

public class SignupValidator : AbstractValidator<SignupCommand>
{
    public SignupValidator()
    {
        RuleFor(x => x.Request.FirstName)
            .NotEmpty();

        RuleFor(x => x.Request.LastName)
            .NotEmpty();

        RuleFor(x => x.Request.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Request.Password)
            .NotEmpty()
            .Matches(x => x.Request.ConformPassword);
    }
}