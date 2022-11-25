using FluentValidation;

namespace Application.CQRS.Managers.Commands.RemoveManagersFromRole;

public class RemoveManagersFromRoleCommandValidator : AbstractValidator<RemoveManagersFromRoleCommand>
{
    public RemoveManagersFromRoleCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty();
        RuleFor(x => x.RoleName).NotEmpty();
    }
}

