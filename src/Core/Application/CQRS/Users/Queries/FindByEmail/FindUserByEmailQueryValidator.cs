using FluentValidation;

namespace Application.CQRS.Users.Queries.FindByEmail;

public class FindUserByEmailQueryValidator : AbstractValidator<FindUserByEmailQuery>
{
    public FindUserByEmailQueryValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}