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

        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CategoryTranslation, CategoryTranslationVM>()
                .ForMember(x => x.MetaTitle, opt => opt.MapFrom(y => y.Meta.Title))
                .ForMember(x => x.MetaKeywords, opt => opt.MapFrom(y => y.Meta.Keywords))
                .ForMember(x => x.MetaDescription, opt => opt.MapFrom(y => y.Meta.Description));
        }
    }
}
