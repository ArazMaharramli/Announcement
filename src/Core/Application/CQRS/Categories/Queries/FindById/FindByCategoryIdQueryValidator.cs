using FluentValidation;

namespace Application.CQRS.Categories.Queries.FindById
{
    public class FindByCategoryIdQueryValidator : AbstractValidator<FindByCategoryIdQuery>
    {
        public FindByCategoryIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
