using System;
using System.Collections.Generic;
using Application.Common.Mappings;
using Application.CQRS.Amenities.Queries.GetAll;
using Application.CQRS.Categories.Queries.GetAll;
using Application.CQRS.Requirements.Queries.GetAll;
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

        public Address Address { get; set; }

        public Contact Contact { get; set; }

        public RoomStatus Status { get; set; }

        public string CategoryId { get; set; }

        public List<AmenitieDetailsVM> Amenities { get; set; }
        public List<RequirementDetailsVM> Requirements { get; set; }
        public List<Media> Medias { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Room, RoomDto>();
        }
    }
}