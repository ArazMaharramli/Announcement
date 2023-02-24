using System.Collections.Generic;
using Application.Common.Mappings;
using Application.CQRS.Amenities.Queries.GetAll;
using Application.CQRS.Categories.Queries.GetAll;
using Application.CQRS.Requirements.Queries.GetAll;
using AutoMapper;
using Domain.Common;
using Domain.Entities;

namespace Application.CQRS.Rooms.Queries.FindBySlug;

public class RoomDetailsVM : IMapFrom<Room>
{
    public string Title { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public string Slug { get; set; }

    public Meta Meta { get; set; }

    public AddressVM Address { get; set; }
    public Contact Contact { get; set; }

    public CategoryDetailsVM Category { get; set; }

    public List<AmenitieDetailsVM> Amenities { get; set; }
    public List<RequirementDetailsVM> Requirements { get; set; }
    public List<MediaVM> Medias { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Room, RoomDetailsVM>();
    }
}
