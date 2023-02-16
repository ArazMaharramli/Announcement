using System;
using Domain.Entities;
using MediatR;

namespace Domain.Events.Searches;

public class SubscriberAddedToSearchDomainEvent : INotification
{
    public Search Search { get; set; }
    public Subscriber AddedSubscriber { get; set; }

    public SubscriberAddedToSearchDomainEvent(Search search, Subscriber addedSubscriber)
    {
        Search = search;
        AddedSubscriber = addedSubscriber;
    }
}


