using FluentValidation;

namespace Application.CQRS.Users.Commands.SendEmailConfirmationCode
{
    public class SendEmailConfirmationCodeCommandValidator : AbstractValidator<SendEmailConfirmationCodeCommand>
    {
        public SendEmailConfirmationCodeCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}

