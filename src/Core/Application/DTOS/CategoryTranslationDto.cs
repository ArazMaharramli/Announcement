using Application.Common.Mappings;
using AutoMapper;
using Domain.Common;
using Domain.Entities;

namespace Application.DTOS
{
    public class CategoryTranslationDto : IMapFrom<CategoryTranslation>
    {
        public string Id { get; set; }
        public string LangCode { get; set; }
        public string Name { get; set; }

        public Meta Meta { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CategoryTranslation, CategoryTranslationDto>()
                .ForMember(x => x.Meta, opt => opt.UseDestinationValue());
        }
    }
}