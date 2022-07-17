﻿using System.Linq;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.CQRS.RoomTypes.Queries.GetAll
{
    public class RoomTypeDetailsVM : IMapFrom<RoomType>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public void Mapping(Profile profile)
        {
            string langCode = "";
            profile.CreateMap<RoomType, RoomTypeDetailsVM>()
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Id))
                .ForMember(x => x.Icon, opt => opt.MapFrom(y => y.Icon))
                .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Translations.FirstOrDefault(x => x.LangCode == langCode).Name));
        }
    }
}
