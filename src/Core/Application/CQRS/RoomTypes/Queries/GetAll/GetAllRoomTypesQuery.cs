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

namespace Application.CQRS.RoomTypes.Queries.GetAll
{
    public class GetAllRoomTypesQuery : IRequest<List<RoomTypeDto>>
    {
        public class Handler : IRequestHandler<GetAllRoomTypesQuery, List<RoomTypeDto>>
        {
            private readonly IDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(IDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public Task<List<RoomTypeDto>> Handle(GetAllRoomTypesQuery request, CancellationToken cancellationToken)
            {
                return _dbContext.RoomTypes
                    .Where(x => !x.Deleted)
                    .ProjectTo<RoomTypeDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
