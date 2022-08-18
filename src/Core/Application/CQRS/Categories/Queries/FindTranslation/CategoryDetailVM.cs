using Application.Common.Mappings;
using AutoMapper;
using Domain.Common;
using Domain.Entities;

namespace Application.CQRS.Categories.Queries.FindTranslation
{
    public class CategoryDetailVM : IMapFrom<CategoryTranslation>
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Lang { get; set; }

        public Meta Meta { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CategoryTranslation, CategoryDetailVM>()
                .ForMember(x => x.Meta, opt => opt.UseDestinationValue())
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Category.Id))
                .ForMember(x => x.Lang, opt => opt.MapFrom(y => y.LangCode))
                .ForMember(x => x.Icon, opt => opt.MapFrom(y => y.Category.Icon));
        }
    }
}

