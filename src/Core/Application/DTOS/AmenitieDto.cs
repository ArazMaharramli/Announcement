using System;
using System.Linq;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOS
{
    public class AmenitieDto : IMapFrom<Amenitie>
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            string lang = "";
            profile.CreateMap<Amenitie, AmenitieDto>()
                .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Translations.FirstOrDefault(x => x.LangCode == lang).Name));
        }
    }
}
