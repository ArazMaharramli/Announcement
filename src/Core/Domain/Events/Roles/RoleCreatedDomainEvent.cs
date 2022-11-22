using System;
using Domain.Entities;
using MediatR;

namespace Domain.Events.Roles;

public class RoleCreatedDomainEvent : INotification
{
    public Role Role { get; private set; }

    public RoleCreatedDomainEvent(Role role)
    {
        Role = role;
    }
}