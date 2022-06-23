using FluentValidation;

namespace Application.CQRS.Requirements.Queries.FindById
{
    public class FindByRequirementIdQueryValidator : AbstractValidator<FindByRequirementIdQuery>
    {
        public FindByRequirementIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
