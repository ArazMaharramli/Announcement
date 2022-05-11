using Common.Validators;
using FluentValidation;

namespace Application.CQRS.Users.ForgotPasword
{
    public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .Must(x => RestrictedEmailProviders.IsCompanyEmail(x));
        }
    }
}
