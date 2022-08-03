using System;
using System.Collections.Generic;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Common;
using Domain.Entities;

namespace Application.DTOS
{
    public class RoomDto : IMapFrom<Room>
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string Slug { get; set; }
        public Meta Meta { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }
        public Gender Gender { get; set; }

        public Address Address { get; set; }

        public Contact Contact { get; set; }

        public RoomStatus Status { get; set; }

        public CategoryDto Category { get; set; }

        public List<AmenitieDto> Amenities { get; set; }
        public List<RequirementDto> Requirements { get; set; }
        public List<Media> Medias { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Room, RoomDto>();
        }
    }
}

