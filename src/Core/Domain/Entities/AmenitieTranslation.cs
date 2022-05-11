using System;
using Domain.Common;

namespace Domain.Entities
{
    public class AmenitieTranslation : Translation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        public string AmenitieId { get; set; }
        public Amenitie Amenitie { get; set; }
    }
}
