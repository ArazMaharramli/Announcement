using FluentValidation;

namespace Application.CQRS.Amenities.Commands.CreateAmenitie
{
    public class CreateAmenitieCommandValidator : AbstractValidator<CreateAmenitieCommand>
    {
        public CreateAmenitieCommandValidator()
        {
            RuleFor(x => x.Translations).NotEmpty();
            RuleFor(x => x.Icon).NotEmpty();
        }
    }
}
