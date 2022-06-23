using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class RoomType : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Icon { get; set; }

        public ICollection<Room> Rooms { get; set; }
        public ICollection<RoomTypeTranslation> Translations { get; set; }

        public RoomType()
        {
            Rooms = new HashSet<Room>();
            Translations = new HashSet<RoomTypeTranslation>();
        }
    }
}
