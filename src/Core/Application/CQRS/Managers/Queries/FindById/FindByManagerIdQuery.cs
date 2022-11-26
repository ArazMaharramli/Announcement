using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOS;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Managers.Queries.FindById;

public class FindByManagerIdQuery : IRequest<ManagerDto>
{
    public string Id { get; set; }

    public class Handler : IRequestHandler<FindByManagerIdQuery, ManagerDto>
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;

        public Handler(IDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ManagerDto> Handle(FindByManagerIdQuery request, CancellationToken cancellationToken)
        {
            var manager = await _dbContext.Managers
                .Include(x => x.Roles)
                .Include(x => x.Claims)
                .AsNoTracking()
                .ProjectTo<ManagerDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (manager is null)
            {
                throw new NotFoundException(nameof(Manager), request.Id);
            }

            return manager;
        }
    }
}
