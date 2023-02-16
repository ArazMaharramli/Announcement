using System.Collections.Generic;
using Domain.Common;
using Domain.Events.Searches;
using NetTopologySuite.Geometries;

namespace Domain.Entities;

public class Search : Entity
{
    public string SearchBox { get; set; }
    public string Query { get; set; }
    public Point Location { get; set; }

    public string CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<Subscriber> Subscribers { get; set; }

    public Search()
    {
        Subscribers = new HashSet<Subscriber>();
    }

    public Search(string searchBox, string query, string categoryId, double? lng, double? lat) : this()
    {
        SearchBox = searchBox?.Trim();
        Query = query?.Trim();
        CategoryId = categoryId;

        if (lat is not null && lng is not null)
        {
            Location = new Point(new Coordinate(lng.Value, lat.Value)) { SRID = 4326 };
        }
    }

    public void AddSubscriber(Subscriber subscriber)
    {
        if (Subscribers.Contains(subscriber))
            return;

        Subscribers.Add(subscriber);

        AddDomainEvent(new SubscriberAddedToSearchDomainEvent(this, subscriber));
    }
}
