using FluentValidation;

namespace Application.CQRS.Requirements.Commands.Update
{
    public class UpdateRequirementCommandValidator : AbstractValidator<UpdateRequirementCommand>
    {
        public UpdateRequirementCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Translations).NotEmpty();
        }
    }
}

