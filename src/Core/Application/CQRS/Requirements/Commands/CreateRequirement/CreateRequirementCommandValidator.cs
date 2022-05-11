using FluentValidation;

namespace Application.CQRS.Requirements.Commands.CreateRequirement
{
    public class CreateRequirementCommandValidator : AbstractValidator<CreateRequirementCommand>
    {
        public CreateRequirementCommandValidator()
        {
            RuleFor(x => x.LangCode).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Icon).NotEmpty();
        }
    }
}
