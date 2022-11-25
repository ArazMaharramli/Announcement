using System;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOS;

public class ManagerDto : IMapFrom<Manager>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }

    public List<RoleDto> Roles { get; set; }
    public List<string> Claims { get; set; }

    public DateTime UpdatedAt { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Manager, ManagerDto>()
            .ForMember(x => x.Claims, opt => opt.MapFrom(z => z.Claims.Select(x => x.ClaimName).ToList()));
    }
}

