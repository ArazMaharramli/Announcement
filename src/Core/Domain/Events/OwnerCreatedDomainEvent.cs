using Domain.Entities;
using MediatR;

namespace Domain.Events
{
    public class OwnerCreatedDomainEvent : INotification
    {
        public Owner Owner { get; set; }

        public OwnerCreatedDomainEvent(Owner owner)
        {
            Owner = owner;
        }
    }
}

