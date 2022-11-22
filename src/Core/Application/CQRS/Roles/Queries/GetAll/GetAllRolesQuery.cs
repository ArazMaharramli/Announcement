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

namespace Application.CQRS.Roles.Queries.GetAll
{
    public class GetAllRolesQuery : IRequest<List<RoleDto>>
    {
        public class Handler : IRequestHandler<GetAllRolesQuery, List<RoleDto>>
        {
            private readonly IDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(IDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public Task<List<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
            {
                return _dbContext.Roles
                    .Include(x => x.Claims)
                    .Where(x => !x.Deleted)
                    .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }

        }

    }
}

