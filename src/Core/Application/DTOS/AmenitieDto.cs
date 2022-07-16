using System;
using System.Collections.Generic;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOS
{
    public class AmenitieDto : IMapFrom<Amenitie>
    {
        public string Id { get; set; }
        public string Icon { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<AmenitieTranslationDto> Translations { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Amenitie, AmenitieDto>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id))
                .ForMember(x => x.Icon, opt => opt.MapFrom(y => y.Icon))
                .ForMember(x => x.Translations, opt => opt.MapFrom(y => y.Translations));
        }
    }
}
