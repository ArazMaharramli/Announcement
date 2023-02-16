using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities;

public class Subscriber : Entity
{
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public bool SubscribedToMarketing { get; set; }

    public ICollection<Search> Searches { get; set; }

    public Subscriber()
    {
        Searches = new HashSet<Search>();
    }

    public Subscriber(string userId, string name, string email, string phone, bool subscribedToMarketing) : this()
    {
        UserId = userId;
        Name = name.Trim();
        Email = email.Trim();
        Phone = phone?.Trim();
        SubscribedToMarketing = subscribedToMarketing;
    }

    public void AddSearch(Search search)
    {
        Searches.Add(search);
    }
}