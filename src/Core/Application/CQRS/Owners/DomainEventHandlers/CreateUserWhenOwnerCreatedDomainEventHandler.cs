using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.CQRS.Owners.IntegrationEvents.Events;
using Domain.Events;
using MediatR;

namespace Application.CQRS.Owners.DomainEventHandlers
{
    public class CreateUserWhenOwnerCreatedDomainEventHandler : INotificationHandler<OwnerCreatedDomainEvent>
    {
        private IUserManager _userManager;
        private readonly IEventBusService _eventBusService;

        public CreateUserWhenOwnerCreatedDomainEventHandler(IUserManager userManager, IEventBusService eventBusService)
        {
            _userManager = userManager;
            _eventBusService = eventBusService;
        }

        public async Task Handle(OwnerCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            var owner = notification.Owner;

            var result = await _userManager.CreateUserAsync(userName: owner.Email, phoneNumber: owner.Phone, email: owner.Email, id: owner.Id);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(';', result.Errors));
            }
            _eventBusService.AddEvent(new OwnerUserCreatedIntegrationEvent(owner));
        }
    }
}

