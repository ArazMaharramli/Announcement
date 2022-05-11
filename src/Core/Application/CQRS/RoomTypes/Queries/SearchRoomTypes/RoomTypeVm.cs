using System;
using System.Linq;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.CQRS.RoomTypes.Queries.SearchRoomTypes
{
    public class RoomTypeVm : IMapFrom<RoomType>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<RoomTypeTranslationVM> Translations { get; set; }
        public void Mapping(Profile profile)
        {
            string lang = "az";
            bool deleted = false;

            profile.CreateMap<RoomType, RoomTypeVm>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id))
                .ForMember(x => x.Image, opt => opt.MapFrom(y => y.Image))
                .ForMember(x => x.Name, opt => opt.MapFrom(
                                                y => y.Translations
                                                        .FirstOrDefault(z => z.LangCode.ToLower() == lang.ToLower() && z.Deleted == deleted).Name ?? "---"))
                .ForMember(x => x.Translations, opt => opt.MapFrom(y => y.Translations));
        }
    }
}
