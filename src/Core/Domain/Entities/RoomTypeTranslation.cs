using System;
using Domain.Common;

namespace Domain.Entities
{
    public class RoomTypeTranslation : Translation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }
        public string Description { get; set; }

        public RoomType RoomType { get; set; }
    }
}
