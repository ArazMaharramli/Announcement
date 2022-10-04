using Application.Common.Mappings;
using AutoMapper;
using Domain.Common;

namespace Application.CQRS.Rooms.Queries.FindBySlug;

public class AddressVM : IMapFrom<Address>
{
    public string AddressLine { get; set; }
    public double Lat { get; set; }
    public double Lng { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Address, AddressVM>()
            .ForMember(x => x.AddressLine, opt => opt.MapFrom(z => z.AddressLine))
            .ForMember(x => x.Lat, opt => opt.MapFrom(z => z.Location.Y))
            .ForMember(x => x.Lng, opt => opt.MapFrom(z => z.Location.X));
    }
}