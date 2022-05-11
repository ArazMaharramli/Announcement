using System.Linq;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.CQRS.Requirements.Commands.AddOrUpdateRequirementTranslation
{
    public class AddOrUpdateRequirementTranslationCommandValidator : AbstractValidator<AddOrUpdateRequirementTranslationCommand>
    {
        public AddOrUpdateRequirementTranslationCommandValidator(IDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(x => dbContext.Requirements.FirstOrDefault(y => y.Id == x) is not null);

            RuleFor(x => x.LangCode).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
