using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.DTOS;

public class RoleDto : IMapFrom<Role>
{
    public string Id { get; set; }
    public string Name { get; set; }

    public DateTime UpdatedAt { get; set; }


    public void Mapping(Profile profile)
    {
        profile.CreateMap<Role, RoleDto>();
    }
}

