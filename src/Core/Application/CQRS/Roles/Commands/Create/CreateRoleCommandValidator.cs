using Application.Common.Interfaces;
using FluentValidation;

namespace Application.CQRS.Roles.Commands.Create
{
    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleCommandValidator(IRoleManager roleManager)
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Must(x => roleManager.FindByName(x).Result is null)
                .WithMessage("This Role already exists");
            RuleFor(x => x.Claims).NotEmpty();
        }
    }
}

