using System;
using System.Collections.Generic;
using System.Linq;
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

        public string GetName(string langCode)
        {
            if (this.Translations.Count <= 0)
            {
                throw new InvalidOperationException("Translations field can not be empty");
            }
            return this.Translations.FirstOrDefault(x => x.LangCode == langCode)?.Name ?? "---";
        }
    }
}
