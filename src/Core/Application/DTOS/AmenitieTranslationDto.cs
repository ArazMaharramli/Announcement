using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOS
{
    public class AmenitieTranslationDto : IMapFrom<AmenitieTranslation>
    {
        public string Id { get; set; }
        public string LangCode { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AmenitieTranslation, AmenitieTranslationDto>()
            .ForMember(x => x.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(x => x.LangCode, opt => opt.MapFrom(s => s.LangCode))
            .ForMember(x => x.Name, opt => opt.MapFrom(s => s.Name));
        }
    }
}
