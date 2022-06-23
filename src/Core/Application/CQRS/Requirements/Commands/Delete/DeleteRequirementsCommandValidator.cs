using System.Linq;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.CQRS.Requirements.Commands.Delete
{
    public class DeleteRequirementsCommandValidator : AbstractValidator<DeleteRequirementsCommand>
    {
        public DeleteRequirementsCommandValidator(IDbContext dbContext)
        {
            RuleFor(x => x.Ids)
                .NotEmpty()
                .Must(x => dbContext.Requirements.Where(y => x.Contains(y.Id)).Count() > 0);
        }
    }
}
