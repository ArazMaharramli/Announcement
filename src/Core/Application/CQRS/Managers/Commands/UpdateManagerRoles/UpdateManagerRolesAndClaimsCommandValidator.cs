using FluentValidation;

namespace Application.CQRS.Managers.Commands.UpdateManagerRoles;

public class UpdateManagerRolesAndClaimsCommandValidator : AbstractValidator<UpdateManagerRolesAndClaimsCommand>
{
    public UpdateManagerRolesAndClaimsCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.RoleIds).NotEmpty();
    }
}

