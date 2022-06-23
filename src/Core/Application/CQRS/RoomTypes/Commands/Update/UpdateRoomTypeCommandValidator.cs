using FluentValidation;

namespace Application.CQRS.RoomTypes.Commands.Update
{
    public class UpdateRoomTypeCommandValidator : AbstractValidator<UpdateRoomTypeCommand>
    {
        public UpdateRoomTypeCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Translations).NotEmpty();
        }
    }
}

