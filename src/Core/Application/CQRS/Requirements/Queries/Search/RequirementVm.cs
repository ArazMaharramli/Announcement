using System;
using System.Linq;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.CQRS.Requirements.Queries.Search
{
    public class RequirementVm : IMapFrom<Requirement>
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<RequirementTranslationVM> Translations { get; set; }

        public void Mapping(Profile profile)
        {
            string lang = "az";
            bool deleted = false;

            profile.CreateMap<Requirement, RequirementVm>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id))
                .ForMember(x => x.Icon, opt => opt.MapFrom(y => y.Icon))
                .ForMember(x => x.Name, opt => opt.MapFrom(
                                                y => y.Translations
                                                        .FirstOrDefault(z => z.LangCode == lang && z.Deleted == deleted).Name ?? "---"))
                .ForMember(x => x.Translations, opt => opt.MapFrom(y => y.Translations));
        }
    }
}
