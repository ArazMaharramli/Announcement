using System;
using System.Linq;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.CQRS.RoomTypes.Queries.Search
{
    public class RoomTypeVm : IMapFrom<RoomType>
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public DateTime UpdatedAt { get; set; }

        public List<RoomTypeTranslationVM> Translations { get; set; }

        public void Mapping(Profile profile)
        {
            string lang = "az";
            bool deleted = false;

            profile.CreateMap<RoomType, RoomTypeVm>()
                .ForMember(x => x.Name, opt => opt.MapFrom(
                                                y => y.Translations
                                                        .FirstOrDefault(z => z.LangCode == lang && z.Deleted == deleted).Name ?? "---"))
                .ForMember(x => x.Translations, opt => opt.MapFrom(y => y.Translations));
        }
    }
}
