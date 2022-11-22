using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Roles.Queries.GetRoleByName
{
    public class GetRoleByNameQuery : IRequest<RoleDto>
    {
        public string Name { get; set; }

        public class Handler : IRequestHandler<GetRoleByNameQuery, RoleDto>
        {
            private readonly IRoleManager _roleManager;
            private readonly IDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(IRoleManager roleManager, IDbContext dbContext, IMapper mapper)
            {
                _roleManager = roleManager;
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public Task<RoleDto> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
            {
                return _dbContext.Roles
                    .Include(x => x.Claims)
                    .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Name || x.Name == request.Name, cancellationToken);

            }

        }

    }
}

