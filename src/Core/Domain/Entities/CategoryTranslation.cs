using System;
using Domain.Common;

namespace Domain.Entities
{
    public class CategoryTranslation : Translation
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Slug { get; set; }
        public string Name { get; set; }

        public Meta Meta { get; set; }

        public string CategoryId { get; set; }
        public Category Category { get; set; }

        public CategoryTranslation()
        {
            Meta = new Meta();
        }
    }
}
