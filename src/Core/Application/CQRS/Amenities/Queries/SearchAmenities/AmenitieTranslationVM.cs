﻿using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.CQRS.Amenities.Queries.SearchAmenities
{
    public class AmenitieTranslationVM : IMapFrom<AmenitieTranslation>
    {
        public string Id { get; set; }
        public string LangCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AmenitieTranslation, AmenitieTranslationVM>()
            .ForMember(x => x.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(x => x.LangCode, opt => opt.MapFrom(s => s.LangCode));
        }
    }
}
