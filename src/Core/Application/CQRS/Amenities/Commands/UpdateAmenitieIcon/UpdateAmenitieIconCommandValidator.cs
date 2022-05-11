using System.Linq;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.CQRS.Amenities.Commands.UpdateAmenitieIcon
{
    public class UpdateAmenitieIconCommandValidator : AbstractValidator<UpdateAmenitieIconCommand>
    {
        public UpdateAmenitieIconCommandValidator(IDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(x => dbContext.Amenities.FirstOrDefault(y => y.Id == x) is not null);

            RuleFor(x => x.Icon).NotEmpty();
        }
    }
}
