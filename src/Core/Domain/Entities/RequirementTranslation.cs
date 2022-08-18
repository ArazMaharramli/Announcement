using System;
using Domain.Common;

namespace Domain.Entities
{
    public class RequirementTranslation : Translation
    {
        public string Name { get; set; }

        public string RequirementId { get; set; }
        public Requirement Requirement { get; set; }
    }
}
