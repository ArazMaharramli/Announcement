using System.Linq;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.CQRS.Requirements.Commands.DeleteRequirement
{
    public class DeleteRequirementCommandValidator : AbstractValidator<DeleteRequirementCommand>
    {
        public DeleteRequirementCommandValidator(IDbContext dbContext)
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .Must(x => dbContext.Requirements.FirstOrDefault(y => y.Id == x) is not null);
        }
    }
}
