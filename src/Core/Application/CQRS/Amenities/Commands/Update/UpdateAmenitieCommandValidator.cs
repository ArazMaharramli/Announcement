using FluentValidation;

namespace Application.CQRS.Amenities.Commands.Update
{
    public class UpdateAmenitieCommandValidator : AbstractValidator<UpdateAmenitieCommand>
    {
        public UpdateAmenitieCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Translations).NotEmpty();
        }
    }
}

