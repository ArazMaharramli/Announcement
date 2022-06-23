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

namespace Application.CQRS.RoomTypes.Queries.FindById
{
    public class FindByRoomTypeIdQuery : IRequest<RoomTypeDto>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<FindByRoomTypeIdQuery, RoomTypeDto>
        {
            private readonly IDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(IDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<RoomTypeDto> Handle(FindByRoomTypeIdQuery request, CancellationToken cancellationToken)
            {
                var roomType = await _dbContext.RoomTypes
                    .Include(x => x.Translations)
                    .ProjectTo<RoomTypeDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (roomType is null)
                {
                    throw new NotFoundException(nameof(RoomType), request.Id);
                }

                return roomType;
            }
        }
    }
}
