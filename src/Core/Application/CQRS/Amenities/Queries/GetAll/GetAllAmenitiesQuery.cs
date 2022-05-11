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

namespace Application.CQRS.Amenities.Queries.GetAll
{
    public class GetAllAmenitiesQuery : IRequest<List<AmenitieDto>>
    {
        public class Handler : IRequestHandler<GetAllAmenitiesQuery, List<AmenitieDto>>
        {
            private readonly IDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(IDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public Task<List<AmenitieDto>> Handle(GetAllAmenitiesQuery request, CancellationToken cancellationToken)
            {
                return _dbContext.Amenities
                    .Where(x => !x.Deleted)
                    .ProjectTo<AmenitieDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
