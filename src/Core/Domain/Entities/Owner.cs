using System.Collections.Generic;
using System.Linq;
using Domain.Common;
using Domain.Events;

namespace Domain.Entities
{
    public class Owner : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ICollection<Room> Rooms { get; set; }

        public Owner()
        {
            Rooms = new HashSet<Room>();
        }

        public Owner(string userId, string name, string email, string phone) : this()
        {
            Id = userId;
            Name = name;
            Email = email;
            Phone = phone;

            AddDomainEvent(new OwnerCreatedDomainEvent(this));
        }

        public void AddRoom(Room room)
        {
            var existingRoom = Rooms.SingleOrDefault(x => x.Id == room.Id || x.Slug == room.Slug);
            if (existingRoom is not null)
            {
                return;
            }
            Rooms.Add(room);
        }
    }
}
