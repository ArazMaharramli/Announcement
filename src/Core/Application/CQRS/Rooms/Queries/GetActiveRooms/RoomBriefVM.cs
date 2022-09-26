using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.CQRS.Rooms.Queries.GetActiveRooms
{
    public class RoomBriefVM : IMapFrom<Room>
    {
        public string Id { get; set; }
        public string MediaUrl { get; set; }
        public string MediaAltTag { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int Price { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Room, RoomBriefVM>();
        }
    }
}

