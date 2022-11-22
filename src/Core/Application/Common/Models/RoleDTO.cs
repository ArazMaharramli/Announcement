using System;
using System.Collections.Generic;
using System.Linq;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Models
{
    public class RoleDto : IMapFrom<Role>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int UsersCount { get; set; }
        public List<string> Claims { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<Role, RoleDto>()
                .ForMember(x => x.UsersCount, opt => opt.MapFrom(x => x.Managers.Count))
                .ForMember(x => x.Claims, opt => opt.MapFrom(x => x.Claims.Select(x => x.ClaimName)));
        }
    }
}

