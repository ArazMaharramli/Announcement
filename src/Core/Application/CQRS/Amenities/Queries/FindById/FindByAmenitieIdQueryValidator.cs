using FluentValidation;

namespace Application.CQRS.Amenities.Queries.FindById
{
    public class FindByAmenitieIdQueryValidator : AbstractValidator<FindByAmenitieIdQuery>
    {
        public FindByAmenitieIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
