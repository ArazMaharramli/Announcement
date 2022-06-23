using System.Linq;
using Application.Common.Interfaces;
using FluentValidation;

namespace Application.CQRS.Categories.Commands.Delete
{
    public class DeleteCategoriesCommandValidator : AbstractValidator<DeleteCategoriesCommand>
    {
        public DeleteCategoriesCommandValidator(IDbContext dbContext)
        {
            RuleFor(x => x.Ids)
                .NotEmpty()
                .Must(x => dbContext.Categories.Where(y => x.Contains(y.Id)).Count() > 0);
        }
    }
}
