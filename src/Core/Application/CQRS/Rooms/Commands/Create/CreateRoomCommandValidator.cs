using FluentValidation;
using Common.Validators;

namespace Application.CQRS.Rooms.Commands.Create;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.ContactName)
            .NotEmpty()
            .OverridePropertyName("Name")
            .WithName("Name");

        RuleFor(x => x.ContactPhone)
            .NotEmpty()
            .Must(x => x.ValidatePhoneNumber())
            .WithName("Phone Number");

        RuleFor(x => x.ContactEmail)
            .NotEmpty()
            .EmailAddress()
            .WithName("Email");

        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThan(0);

        RuleFor(x => x.Address)
            .NotEmpty();

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithName("Category");

        RuleFor(x => x.AmenitieIds)
            .NotEmpty()
            .WithName("Amenities");

        RuleFor(x => x.RequirementIds)
            .NotEmpty()
            .WithName("Requirements");

        RuleFor(x => x.MediaUrls)
            .NotEmpty()
            .WithName("Medias");
    }
}

