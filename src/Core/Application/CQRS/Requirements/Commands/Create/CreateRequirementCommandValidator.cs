using FluentValidation;

namespace Application.CQRS.Requirements.Commands.Create
{
    public class CreateRequirementCommandValidator : AbstractValidator<CreateRequirementCommand>
    {
        public CreateRequirementCommandValidator()
        {
            RuleFor(x => x.Translations).NotEmpty();
            RuleFor(x => x.Icon).NotEmpty();
        }
    }
}
