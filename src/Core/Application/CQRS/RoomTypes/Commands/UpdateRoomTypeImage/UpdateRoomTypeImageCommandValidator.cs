using System.Linq;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.CQRS.RoomTypes.Commands.UpdateRoomTypeImage
{
    public class UpdateRoomTypeImageCommandValidator : AbstractValidator<UpdateRoomTypeImageCommand>
    {
        public UpdateRoomTypeImageCommandValidator(IDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(x => dbContext.RoomTypes.FirstOrDefault(y => y.Id == x) is not null);

            RuleFor(x => x.Image).NotEmpty();
        }
    }
}
