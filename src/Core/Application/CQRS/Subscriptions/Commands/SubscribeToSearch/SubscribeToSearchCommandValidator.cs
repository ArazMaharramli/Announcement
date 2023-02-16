using Common.Validators;
using FluentValidation;

namespace Application.CQRS.Subscriptions.Commands.SubscribeToSearch;

public class SubscribeToSearchCommandValidator : AbstractValidator<SubscribeToSearchCommand>
{
    public SubscribeToSearchCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(x => x.Phone)
            .Must(x => PhoneNumberValidator.ValidatePhoneNumber(x))
            .When(x=>!string.IsNullOrEmpty(x.Phone));

    }
}

