using FluentValidation;
using Common.Validators;

namespace Application.CQRS.Rooms.Commands.Create
{
    public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
    {
        public CreateRoomCommandValidator()
        {
            RuleFor(x => x.ContactName).NotEmpty();
            RuleFor(x => x.ContactPhone).NotEmpty().Must(x => x.ValidatePhoneNumber());
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0);

            RuleFor(x => x.Address).NotEmpty();

            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.RoomTypeId).NotEmpty();
            RuleFor(x => x.AmenitieIds).NotEmpty();
            RuleFor(x => x.RequirementIds).NotEmpty();
            RuleFor(x => x.MediaUrls).NotEmpty();
        }
    }
}

