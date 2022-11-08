using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models.ConfigModels;
using Application.CQRS.Owners.Events.IntegrationEvents;
using Application.CQRS.Users.Commands.Register;
using MediatR;

namespace Application.CQRS.Owners.EventHandlers.IntegrationEventHandlers.OwnerCreated
{
    public class RegisterUserWhenOwnerCreatedIntegrationEventhandler : INotification
    {
        public class Handler : INotificationHandler<OwnerCreatedIntegrationEvent>
        {
            private readonly IMediator _mediator;
            private readonly TenantInfo _tenantInfo;
            public Handler(IMediator mediator, TenantInfo tenantInfo)
            {
                _mediator = mediator;
                _tenantInfo = tenantInfo;
            }

            public async Task Handle(OwnerCreatedIntegrationEvent notification, CancellationToken cancellationToken)
            {
                await _mediator.Send(new RegisterUserCommand
                {
                    Id = notification.Owner.Id,
                    Name = notification.Owner.Name,
                    Phone = notification.Owner.Phone,
                    Email = notification.Owner.Email,
                }, cancellationToken);
            }
        }
    }
}

