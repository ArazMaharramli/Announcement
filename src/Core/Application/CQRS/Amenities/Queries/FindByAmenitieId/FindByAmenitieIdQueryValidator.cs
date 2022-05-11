using FluentValidation;

namespace Application.CQRS.Amenities.Queries.FindByAmenitieId
{
    public class FindByAmenitieIdQueryValidator : AbstractValidator<FindByAmenitieIdQuery>
    {
        public FindByAmenitieIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
