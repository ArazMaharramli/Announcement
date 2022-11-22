using Domain.Entities;
using MediatR;

namespace Domain.Events.Roles;

public class RoleUpdatedDomainEvent : INotification
{
    public Role Role { get; private set; }

    public RoleUpdatedDomainEvent(Role role)
    {
        Role = role;
    }
}

