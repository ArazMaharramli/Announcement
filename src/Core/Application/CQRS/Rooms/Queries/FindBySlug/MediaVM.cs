using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.CQRS.Rooms.Queries.FindBySlug;

public class MediaVM : IMapFrom<Media>
{
    public string Url { get; set; }
    public string Alt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Media, MediaVM>()
            .ForMember(x => x.Url, opt => opt.MapFrom(z => z.Url))
            .ForMember(x => x.Alt, opt => opt.MapFrom(z => z.AltTag));
    }
}