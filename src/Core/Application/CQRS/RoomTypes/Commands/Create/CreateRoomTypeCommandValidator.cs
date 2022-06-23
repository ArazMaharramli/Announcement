using FluentValidation;

namespace Application.CQRS.RoomTypes.Commands.Create
{
    public class CreateRoomTypeCommandValidator : AbstractValidator<CreateRoomTypeCommand>
    {
        public CreateRoomTypeCommandValidator()
        {
            RuleFor(x => x.Translations).NotEmpty();
            RuleFor(x => x.Icon).NotEmpty();
        }
    }
}
