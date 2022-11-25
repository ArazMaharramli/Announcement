using System;
using Application.Common.Models;
using MediatR;

namespace Application.CQRS.Managers.IntegrationEvents.ManagerDeleted;

public class ManagerDeletedIntegrationEvent : IntegrationEvent, INotification
{
    public string Id { get; set; }

    public ManagerDeletedIntegrationEvent(string id)
    {
        Id = id;
    }
}

