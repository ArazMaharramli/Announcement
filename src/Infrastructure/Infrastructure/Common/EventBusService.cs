using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Infrastructure.Common
{
    public class EventBusService : IEventBusService
    {
        private readonly IMediator _mediator;

        public EventBusService(IMediator mediator) : this()
        {
            _mediator = mediator;
        }

        public EventBusService()
        {
            _events = new List<IntegrationEvent>();
        }

        private List<IntegrationEvent> _events;

        public void AddEvent(IntegrationEvent @event)
        {
            _events.Add(@event);
        }

        public void Clear()
        {
            _events.Clear();
        }

        public void RemoveEvent(IntegrationEvent @event)
        {
            _events.Remove(@event);
        }

        public async Task PublishAsync()
        {
            foreach (var item in _events)
            {
                await _mediator.Publish(item);
            }
            _events.Clear();
        }
    }
}

