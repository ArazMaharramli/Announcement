using FluentValidation;

namespace Application.CQRS.Managers.Queries.FindById;

public class FindByManagerIdQueryValidator : AbstractValidator<FindByManagerIdQuery>
{
    public FindByManagerIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
