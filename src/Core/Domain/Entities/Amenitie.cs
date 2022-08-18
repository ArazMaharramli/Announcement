using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Amenitie : Entity
    {
        public string Icon { get; set; }

        public ICollection<Room> Rooms { get; set; }
        public ICollection<AmenitieTranslation> Translations { get; set; }

        public Amenitie()
        {
            Rooms = new HashSet<Room>();
            Translations = new HashSet<AmenitieTranslation>();
        }
    }
}
