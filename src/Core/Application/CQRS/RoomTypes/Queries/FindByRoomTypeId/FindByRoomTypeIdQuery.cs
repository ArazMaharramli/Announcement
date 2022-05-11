using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOS;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.RoomTypes.Queries.FindByRoomTypeId
{
    public class FindByRoomTypeIdQuery : IRequest<RoomTypeDto>
    {
        public string Id { get; set; }
        public string LangCode { get; set; }
        public bool Deleted { get; set; }

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
                    .Include(x => x.Translations.Where(y => !y.Deleted && y.LangCode == request.LangCode))
                    .FirstOrDefaultAsync(x => x.Deleted == request.Deleted && x.Id == request.Id);

                return _mapper.Map<RoomTypeDto>(roomType);
            }
        }
    }
}
