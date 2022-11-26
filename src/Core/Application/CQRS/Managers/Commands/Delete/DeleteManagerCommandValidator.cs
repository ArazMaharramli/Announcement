using FluentValidation;

namespace Application.CQRS.Managers.Commands.Delete;

public class DeleteManagerCommandValidator : AbstractValidator<DeleteManagersCommand>
{
    public DeleteManagerCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty();
    }
}
