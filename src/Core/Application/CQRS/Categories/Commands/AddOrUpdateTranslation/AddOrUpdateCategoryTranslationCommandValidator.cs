using System.Linq;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.CQRS.Categories.Commands.AddOrUpdateTranslation
{
    public class AddOrUpdateCategoryTranslationCommandValidator : AbstractValidator<AddOrUpdateCategoryTranslationCommand>
    {
        public AddOrUpdateCategoryTranslationCommandValidator(IDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(x => dbContext.Categories.FirstOrDefault(y => y.Id == x) is not null);

            RuleFor(x => x.LangCode).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
