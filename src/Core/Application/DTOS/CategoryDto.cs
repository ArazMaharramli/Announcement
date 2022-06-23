using System;
using System.Collections.Generic;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOS
{
    public class CategoryDto : IMapFrom<Category>
    {
        public string Id { get; set; }
        public string Icon { get; set; }

        public DateTime UpdatedAt { get; set; }

        public List<CategoryTranslationDto> Translations { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Category, CategoryDto>()
                .ForMember(x => x.Translations, opt => opt.MapFrom(y => y.Translations));
        }
    }
}