using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models.ConfigModels;
using Application.CQRS.Rooms.IntegrationEvents.Events;
using MediatR;

namespace Application.CQRS.Rooms.IntegrationEvents.Handlers
{
    public class SendEmailToOwnerWhenRoomCreatedIntegrationEventHandler : INotificationHandler<RoomCreatedIntegrationEvent>
    {
        private readonly IEmailService _emailService;
        private readonly EmailTemplates _emailTemplates;
        private readonly StaticUrls _staticUrls;

        public SendEmailToOwnerWhenRoomCreatedIntegrationEventHandler(StaticUrls staticUrls, TenantInfo tenant, IEmailService emailService)
        {
            _staticUrls = staticUrls;
            _emailTemplates = tenant.EmailTemplates;
            _emailService = emailService;
        }

        public Task Handle(RoomCreatedIntegrationEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}