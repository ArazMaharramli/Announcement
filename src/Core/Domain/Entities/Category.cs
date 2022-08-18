using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Category : Entity
    {
        public string Icon { get; set; }

        public ICollection<Room> Rooms { get; set; }
        public ICollection<CategoryTranslation> Translations { get; set; }

        public Category()
        {
            Rooms = new HashSet<Room>();
            Translations = new HashSet<CategoryTranslation>();
        }
    }
}
