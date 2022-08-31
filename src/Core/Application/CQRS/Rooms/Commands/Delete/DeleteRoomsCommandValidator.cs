using FluentValidation;

namespace Application.CQRS.Rooms.Commands.Delete
{
    public class DeleteRoomsCommandValidator : AbstractValidator<DeleteRoomsCommand>
    {
        public DeleteRoomsCommandValidator()
        {
            RuleFor(x => x.Ids).NotEmpty();
        }
    }
}

