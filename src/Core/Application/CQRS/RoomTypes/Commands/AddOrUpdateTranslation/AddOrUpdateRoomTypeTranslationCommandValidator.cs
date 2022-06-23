using System.Linq;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.CQRS.RoomTypes.Commands.AddOrUpdateTranslation
{
    public class AddOrUpdateRoomTypeTranslationCommandValidator : AbstractValidator<AddOrUpdateRoomTypeTranslationCommand>
    {
        public AddOrUpdateRoomTypeTranslationCommandValidator(IDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(x => dbContext.RoomTypes.FirstOrDefault(y => y.Id == x) is not null);

            RuleFor(x => x.LangCode).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
