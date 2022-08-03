using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Room : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }
        public string Slug { get; set; }
        public Meta Meta { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }
        public Gender Gender { get; set; }

        public Address Address { get; set; }

        public Contact Contact { get; set; }

        public RoomStatus Status { get; set; }
        public string AdminNotes { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<Amenitie> Amenities { get; set; }
        public ICollection<Requirement> Requirements { get; set; }
        public ICollection<Media> Medias { get; set; }

        public Room()
        {
            Requirements = new HashSet<Requirement>();
            Amenities = new HashSet<Amenitie>();
            Medias = new HashSet<Media>();
        }
    }
}
