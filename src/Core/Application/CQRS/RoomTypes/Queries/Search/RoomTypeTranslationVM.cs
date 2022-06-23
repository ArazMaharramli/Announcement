using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.CQRS.RoomTypes.Queries.Search
{
    public class RoomTypeTranslationVM : IMapFrom<RoomTypeTranslation>
    {
        public string Id { get; set; }
        public string LangCode { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RoomTypeTranslation, RoomTypeTranslationVM>();
        }
    }
}
