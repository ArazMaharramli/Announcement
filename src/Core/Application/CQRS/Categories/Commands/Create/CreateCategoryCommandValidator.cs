using FluentValidation;

namespace Application.CQRS.Categories.Commands.Create
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Translations).NotEmpty();
            RuleFor(x => x.Icon).NotEmpty();
        }
    }
}
