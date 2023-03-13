using System.Linq;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.CQRS.Rooms.Commands.UpdateByOwner;

public class UpdateRoomByOwnerCommandValidator : AbstractValidator<UpdateRoomByOwnerCommand>
{
    public UpdateRoomByOwnerCommandValidator(IDbContext dbContext, ICurrentUserService currentUserService)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .Must(x => dbContext.Rooms.Any(z => z.Id == x && z.OwnerId == currentUserService.UserId));

        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Price).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.AddressLine).NotEmpty();
        //RuleFor(x => x.Lat).NotEmpty();
        //RuleFor(x => x.Lng).NotEmpty();
        RuleFor(x => x.Contact).NotEmpty();
    }
}
