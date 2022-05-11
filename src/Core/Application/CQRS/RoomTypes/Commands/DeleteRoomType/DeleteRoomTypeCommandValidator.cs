using System.Linq;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.CQRS.RoomTypes.Commands.DeleteRoomType
{
    public class DeleteRoomTypeCommandValidator : AbstractValidator<DeleteRoomTypeCommand>
    {
        public DeleteRoomTypeCommandValidator(IDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(x => dbContext.RoomTypes.FirstOrDefault(y => y.Id == x) is not null);
        }
    }
}
