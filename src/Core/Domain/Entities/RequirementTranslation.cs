using System;
using Domain.Common;

namespace Domain.Entities
{
    public class RequirementTranslation : Translation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }

        public string RequirementId { get; set; }
        public Requirement Requirement { get; set; }
    }
}
