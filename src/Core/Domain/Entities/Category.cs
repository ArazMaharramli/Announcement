using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities
{
    public class Category : AuditableEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
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
