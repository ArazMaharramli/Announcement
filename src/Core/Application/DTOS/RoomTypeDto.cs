using System.Linq;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOS
{
    public class RoomTypeDto : IMapFrom<RoomType>
    {
        public string Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RoomType, RoomTypeDto>()
                .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Translations.FirstOrDefault().Name));
        }
    }
}
