using AutoMapper;
using Domain.Entities;
using Application.Common.Mappings;
using System.Linq;
using System;
using Domain.Common;

namespace Application.CQRS.Rooms.Queries.Search;

public class RoomVm : IMapFrom<Room>
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string MediaUrl { get; set; }

    public RoomStatus Status { get; set; }

    public DateTime UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Room, RoomVm>()
            .ForMember(x => x.MediaUrl, opt => opt.MapFrom(z => z.Medias.FirstOrDefault().Url));
    }
}