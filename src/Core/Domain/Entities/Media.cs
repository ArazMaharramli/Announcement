using System;

namespace Domain.Entities
{
    public class Media
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Url { get; set; }
        public string AltTag { get; set; }
        public string ContentId { get; set; }
    }
}
