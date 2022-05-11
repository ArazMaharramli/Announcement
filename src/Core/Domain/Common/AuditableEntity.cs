using System;
namespace Domain.Common
{
    public class AuditableEntity
    {
        public bool Deleted { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
