using System;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOS
{
    public class RequirementDto : IMapFrom<Requirement>
    {
        public string Id { get; set; }
        public string Icon { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<RequirementTranslationDto> Translations { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Requirement, RequirementDto>()
                .ForMember(x => x.Translations, opt => opt.MapFrom(y => y.Translations));
        }
    }
}