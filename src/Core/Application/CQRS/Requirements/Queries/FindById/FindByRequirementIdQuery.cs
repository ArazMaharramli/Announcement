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

namespace Application.CQRS.Requirements.Queries.FindById
{
    public class FindByRequirementIdQuery : IRequest<RequirementDto>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<FindByRequirementIdQuery, RequirementDto>
        {
            private readonly IDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(IDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<RequirementDto> Handle(FindByRequirementIdQuery request, CancellationToken cancellationToken)
            {
                var requirement = await _dbContext.Requirements
                    .Include(x => x.Translations)
                    .ProjectTo<RequirementDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (requirement is null)
                {
                    throw new NotFoundException(nameof(Requirement), request.Id);
                }

                return requirement;
            }
        }
    }
}
