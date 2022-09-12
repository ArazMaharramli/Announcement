using FluentValidation;

namespace Application.CQRS.Rooms.Commands.Decline;

public class DeclineRoomCommandValidator : AbstractValidator<DeclineRoomCommand>
{
    public DeclineRoomCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Notes).NotEmpty();
    }
}

