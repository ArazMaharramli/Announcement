using System;
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
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Requirement, RequirementDto>()
                .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Translations.FirstOrDefault().Name));
        }
    }
}