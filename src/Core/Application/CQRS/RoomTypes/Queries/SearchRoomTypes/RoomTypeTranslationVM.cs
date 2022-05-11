using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.CQRS.RoomTypes.Queries.SearchRoomTypes
{
    public class RoomTypeTranslationVM : IMapFrom<RoomTypeTranslation>
    {
        public string Id { get; set; }
        public string LangCode { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RoomTypeTranslation, RoomTypeTranslationVM>()
            .ForMember(x => x.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(x => x.LangCode, opt => opt.MapFrom(s => s.LangCode));
        }
    }
}
