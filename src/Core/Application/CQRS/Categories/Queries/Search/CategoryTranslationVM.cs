using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.CQRS.Categories.Queries.Search
{
    public class CategoryTranslationVM : IMapFrom<CategoryTranslation>
    {
        public string Id { get; set; }
        public string LangCode { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CategoryTranslation, CategoryTranslationVM>();
        }
    }
}
