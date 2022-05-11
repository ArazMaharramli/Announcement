using FluentValidation;

namespace Application.CQRS.RoomTypes.Commands.CreateRoomType
{
    public class CreateRoomTypeCommandValidator : AbstractValidator<CreateRoomTypeCommand>
    {
        public CreateRoomTypeCommandValidator()
        {
            RuleFor(x => x.LangCode).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Image).NotEmpty();
        }
    }
}
