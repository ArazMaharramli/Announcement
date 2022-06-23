using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOS;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Requirements.Queries.GetAll
{
    public class GetAllRequirementsQuery : IRequest<List<RequirementDto>>
    {
        public class Handler : IRequestHandler<GetAllRequirementsQuery, List<RequirementDto>>
        {
            private readonly IDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(IDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public Task<List<RequirementDto>> Handle(GetAllRequirementsQuery request, CancellationToken cancellationToken)
            {
                return _dbContext.Requirements
                    .Where(x => !x.Deleted)
                    .ProjectTo<RequirementDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
