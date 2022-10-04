using FluentValidation;

namespace Application.CQRS.Rooms.Queries.FindBySlug;

public class FindRoomBySlugQueryValidator : AbstractValidator<FindRoomBySlugQuery>
{
    public FindRoomBySlugQueryValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}
