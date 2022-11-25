using FluentValidation;

namespace Application.CQRS.Managers.Commands.Create;

public class CreateManagerCommandValidator : AbstractValidator<CreateManagerCommand>
{
    public CreateManagerCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.RoleIds).NotEmpty();
    }
}
