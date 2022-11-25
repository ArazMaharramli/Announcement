using FluentValidation;

namespace Application.CQRS.Managers.Commands.Delete;

public class DeleteManagerCommandValidator : AbstractValidator<DeleteManagerCommand>
{
    public DeleteManagerCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty();
    }
}
