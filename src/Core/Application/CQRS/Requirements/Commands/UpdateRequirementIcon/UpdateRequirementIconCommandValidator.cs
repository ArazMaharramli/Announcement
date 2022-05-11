using System.Linq;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.CQRS.Requirements.Commands.UpdateRequirementIcon
{
    public class UpdateRequirementIconCommandValidator : AbstractValidator<UpdateRequirementIconCommand>
    {
        public UpdateRequirementIconCommandValidator(IDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(x => dbContext.Requirements.FirstOrDefault(y => y.Id == x) is not null);

            RuleFor(x => x.Icon).NotEmpty();
        }
    }
}
