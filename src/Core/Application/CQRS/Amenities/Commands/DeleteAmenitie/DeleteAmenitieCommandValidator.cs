using System.Linq;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.CQRS.Amenities.Commands.DeleteAmenitie
{
    public class DeleteAmenitieCommandValidator : AbstractValidator<DeleteAmenitieCommand>
    {
        public DeleteAmenitieCommandValidator(IDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(x => dbContext.Amenities.FirstOrDefault(y => y.Id == x) is not null);
        }
    }
}
