using FluentValidation;

namespace Application.CQRS.RoomTypes.Queries.FindById
{
    public class FindByRoomTypeIdQueryValidator : AbstractValidator<FindByRoomTypeIdQuery>
    {
        public FindByRoomTypeIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
