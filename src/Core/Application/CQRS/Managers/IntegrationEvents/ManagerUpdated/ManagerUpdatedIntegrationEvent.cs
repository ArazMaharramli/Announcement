using System;
using Application.Common.Models;
using MediatR;

namespace Application.CQRS.Managers.IntegrationEvents.ManagerUpdated;

public class ManagerUpdatedIntegrationEvent : IntegrationEvent, INotification
{
    public ManagerUpdatedIntegrationEvent(string id, string name, string email, string phone)
    {
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
    }

    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
}
