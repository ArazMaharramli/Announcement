using System.Linq;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.CQRS.Amenities.Commands.Delete
{
    public class DeleteAmenitiesCommandValidator : AbstractValidator<DeleteAmenitiesCommand>
    {
        public DeleteAmenitiesCommandValidator(IDbContext dbContext)
        {
            RuleFor(x => x.Ids)
                .NotEmpty()
                .Must(x => dbContext.Amenities.Where(y => x.Contains(y.Id)).Count() > 0);
        }
    }
}
