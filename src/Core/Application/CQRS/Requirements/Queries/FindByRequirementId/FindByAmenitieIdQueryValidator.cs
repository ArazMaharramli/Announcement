using FluentValidation;

namespace Application.CQRS.Requirements.Queries.FindByRequirementId
{
    public class FindByRequirementIdQueryValidator : AbstractValidator<FindByRequirementIdQuery>
    {
        public FindByRequirementIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
