using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Common;
using Domain.Events;

namespace Domain.Entities
{
    public class Room : Entity
    {
        public string Title { get; set; }
        public string Slug { get; set; }
        public Meta Meta { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public Address Address { get; set; }

        public Contact Contact { get; set; }

        public RoomStatus Status { get; set; }
        public string AdminNotes { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }

        public string OwnerId { get; set; }
        public Owner Owner { get; set; }

        public ICollection<Amenitie> Amenities { get; set; }
        public ICollection<Requirement> Requirements { get; set; }
        public ICollection<Media> Medias { get; set; }

        public Room()
        {
            Requirements = new HashSet<Requirement>();
            Amenities = new HashSet<Amenitie>();
            Medias = new HashSet<Media>();
        }

        public Room(
            string title,
            string slug,
            string description,
            int price,
            string address,
            double lng,
            double lat,
            string contactPhone,
            string contactName,
            string contactEmail,
            List<string> mediaUrls,
            string metaKeywords,
            string categoryId,
            List<Amenitie> amenities,
            List<Requirement> requirements) : this()
        {
            Title = title;
            Slug = slug;

            Description = description;
            Price = price;

            Address = new Address
            {
                AddressLine = address,
                Location = new NetTopologySuite.Geometries.Point(new NetTopologySuite.Geometries.Coordinate(lng, lat)) { SRID = 4326 }
            };
            Contact = new Contact
            {
                Phone = contactPhone,
                Name = contactName,
                Email = contactEmail
            };

            Medias = mediaUrls.Select(x => new Media
            {
                Url = x,
                AltTag = $"{Title} - image",
            }).ToList();

            Meta = new Meta
            {
                Title = title,
                Description = Description,
                Keywords = metaKeywords
            };

            CategoryId = categoryId;

            Amenities = amenities;
            Requirements = requirements;

            Status = RoomStatus.PendingConfirmation;

            AddRoomCreatedDomainEvent();
        }
        public void Publish()
        {
            _setStatus(RoomStatus.Active);
        }

        public void Decline(string note)
        {
            AdminNotes = note.Trim();
            _setStatus(RoomStatus.Declined);
        }

        public void Archieve()
        {
            _setStatus(RoomStatus.Archieved);
        }

        private void _setStatus(RoomStatus status)
        {
            Status = status;
        }
        private void AddRoomCreatedDomainEvent()
        {
            var roomCreatedDomainEvent = new RoomCreatedDomainEvent(this);

            this.AddDomainEvent(roomCreatedDomainEvent);
        }


    }
}
