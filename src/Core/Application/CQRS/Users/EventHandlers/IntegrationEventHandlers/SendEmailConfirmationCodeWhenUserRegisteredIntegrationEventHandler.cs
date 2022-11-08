using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models.ConfigModels;
using Application.CQRS.Users.Commands.SendEmailConfirmationCode;
using Application.CQRS.Users.Events.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Options;

namespace Application.CQRS.Users.EventHandlers.IntegrationEventHandlers
{
    public class SendEmailConfirmationCodeWhenUserRegisteredIntegrationEventHandler : INotificationHandler<UserRegisteredIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public SendEmailConfirmationCodeWhenUserRegisteredIntegrationEventHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Handle(UserRegisteredIntegrationEvent notification, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(notification.User.Email))
            {
                await _mediator.Publish(new SendEmailConfirmationCodeCommand { UserId = notification.User.Id }, cancellationToken);
            }
        }
    }
}

