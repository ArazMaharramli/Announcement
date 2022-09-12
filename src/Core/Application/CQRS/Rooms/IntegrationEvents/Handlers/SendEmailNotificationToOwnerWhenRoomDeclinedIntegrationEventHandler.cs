using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models.ConfigModels;
using Application.CQRS.Rooms.IntegrationEvents.Events;
using MediatR;

namespace Application.CQRS.Rooms.IntegrationEvents.Handlers;

public class SendEmailNotificationToOwnerWhenRoomDeclinedIntegrationEventHandler : INotificationHandler<RoomDeclinedIntegrationEvent>
{
    private readonly IEmailService _emailService;
    private readonly EmailTemplates _emailTemplates;
    private readonly StaticUrls _staticUrls;

    public SendEmailNotificationToOwnerWhenRoomDeclinedIntegrationEventHandler(IEmailService emailService, StaticUrls staticUrls, EmailTemplates emailTemplates)
    {
        _emailService = emailService;
        _staticUrls = staticUrls;
        _emailTemplates = emailTemplates;
    }


    public Task Handle(RoomDeclinedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        //var link = string.Format(_staticUrls.RoomDeclined,);
        //await _emailService.SendEmailAsync(user.Email, _emailTemplates.UserEmailConfirmation.Subject, string.Format(_emailTemplates.UserEmailConfirmation.Body, owner.Name, token, link));
        return Task.CompletedTask;
    }
}

