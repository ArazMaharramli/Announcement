using FluentValidation;

namespace Application.CQRS.Rooms.Queries.FindById;

public class FindByRoomIdQueryValidator : AbstractValidator<FindByRoomIdQuery>
{
    public FindByRoomIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

