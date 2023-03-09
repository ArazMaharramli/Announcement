using System;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOS;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using FluentValidation;

namespace Application.CQRS.Rooms.Queries.FindById;

public class FindByRoomIdQuery : IRequest<RoomDto>
{
    public string Id { get; set; }

    public class Handler : IRequestHandler<FindByRoomIdQuery, RoomDto>
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentLanguageService _currentLanguageService;

        public Handler(IDbContext dbContext, IMapper mapper, ICurrentLanguageService currentLanguageService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _currentLanguageService = currentLanguageService;
        }

        public async Task<RoomDto> Handle(FindByRoomIdQuery request, CancellationToken cancellationToken)
        {
            var room = await _dbContext.Rooms
                .Include(x => x.Amenities)
                    .ThenInclude(x => x.Translations.Where(x => x.LangCode == _currentLanguageService.LangCode))
                .Include(x => x.Requirements)
                    .ThenInclude(x => x.Translations.Where(x => x.LangCode == _currentLanguageService.LangCode))
                .AsNoTracking()
                .IgnoreQueryFilters()
                .ProjectTo<RoomDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (room is null)
            {
                throw new NotFoundException(nameof(Room), request.Id);
            }

            return room;
        }
    }
}

