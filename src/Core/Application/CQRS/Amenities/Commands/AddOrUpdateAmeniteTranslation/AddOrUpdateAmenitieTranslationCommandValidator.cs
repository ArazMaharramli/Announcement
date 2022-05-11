using System.Linq;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.CQRS.Amenities.Commands.AddOrUpdateAmenitieTranslation
{
    public class AddOrUpdateAmenitieTranslationCommandValidator : AbstractValidator<AddOrUpdateAmenitieTranslationCommand>
    {
        public AddOrUpdateAmenitieTranslationCommandValidator(IDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(x => dbContext.Amenities.FirstOrDefault(y => y.Id == x) is not null);

            RuleFor(x => x.LangCode).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
