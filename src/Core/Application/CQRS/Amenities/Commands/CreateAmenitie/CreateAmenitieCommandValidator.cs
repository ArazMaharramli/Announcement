using FluentValidation;

namespace Application.CQRS.Amenities.Commands.CreateAmenitie
{
    public class CreateAmenitieCommandValidator : AbstractValidator<CreateAmenitieCommand>
    {
        public CreateAmenitieCommandValidator()
        {
            RuleFor(x => x.LangCode).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Icon).NotEmpty();
        }
    }
}
