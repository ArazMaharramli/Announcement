using System.Linq;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.CQRS.RoomTypes.Commands.Delete
{
    public class DeleteRoomTypesCommandValidator : AbstractValidator<DeleteRoomTypesCommand>
    {
        public DeleteRoomTypesCommandValidator(IDbContext dbContext)
        {
            RuleFor(x => x.Ids)
                .NotEmpty()
                .Must(x => dbContext.RoomTypes.Where(y => x.Contains(y.Id)).Count() > 0);
        }
    }
}
