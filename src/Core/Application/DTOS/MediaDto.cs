using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOS
{
    public class MediaDto : IMapFrom<Media>
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string AltTag { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Media, MediaDto>();
        }
    }
}

