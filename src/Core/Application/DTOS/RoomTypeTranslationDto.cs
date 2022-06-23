using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOS
{
    public class RoomTypeTranslationDto : IMapFrom<RoomTypeTranslation>
    {
        public string Id { get; set; }
        public string LangCode { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RoomTypeTranslation, RoomTypeTranslationDto>();
        }
    }
}