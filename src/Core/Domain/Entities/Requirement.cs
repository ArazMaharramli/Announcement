using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Requirement : Entity
    {
        public string Icon { get; set; }

        public ICollection<Room> Rooms { get; set; }
        public ICollection<RequirementTranslation> Translations { get; set; }

        public Requirement()
        {
            Rooms = new HashSet<Room>();
            Translations = new HashSet<RequirementTranslation>();
        }
    }
}
