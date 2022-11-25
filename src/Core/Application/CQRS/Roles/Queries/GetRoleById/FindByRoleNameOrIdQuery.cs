using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Roles.Queries.GetRoleById;

public class FindByRoleNameOrIdQuery : IRequest<RoleDto>
{
    public string Id { get; set; }

    public class Handler : IRequestHandler<FindByRoleNameOrIdQuery, RoleDto>
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;

        public Handler(IDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<RoleDto> Handle(FindByRoleNameOrIdQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Roles
                .Include(x => x.Claims)
                .Include(x => x.Managers)
                .ProjectTo<RoleDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id || x.Name == request.Id, cancellationToken);
        }

    }

}

