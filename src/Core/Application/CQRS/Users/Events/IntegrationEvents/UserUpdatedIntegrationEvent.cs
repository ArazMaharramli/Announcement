using Application.Common.Models;
using MediatR;

namespace Application.CQRS.Users.Events.IntegrationEvents;

public class UserUpdatedIntegrationEvent : IntegrationEvent, INotification
{
    public UserDTO User { get; set; }

    public UserUpdatedIntegrationEvent(UserDTO user)
    {
        User = user;
    }
}
