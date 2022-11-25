using Common.Validators;
using FluentValidation;

namespace Application.CQRS.Managers.Commands.Update;

public class UpdateManagerCommandValidator : AbstractValidator<UpdateManagerCommand>
{
    public UpdateManagerCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Phone).Must(x => PhoneNumberValidator.ValidatePhoneNumber(x));
        RuleFor(x => x.RoleIds).NotEmpty();
    }
}
