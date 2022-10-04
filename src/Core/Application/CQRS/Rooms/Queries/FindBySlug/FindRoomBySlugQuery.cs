using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Rooms.Queries.FindBySlug;

public class FindRoomBySlugQuery : IRequest<RoomDetailsVM>
{
    public string Slug { get; set; }
    public class Handler : IRequestHandler<FindRoomBySlugQuery, RoomDetailsVM>
    {
        private readonly IDbContext _dbContext;
        private readonly ICurrentLanguageService _currentLanguageService;
        private readonly IMapper _mapper;

        public Handler(IDbContext dbContext, ICurrentLanguageService currentLanguageService, IMapper mapper)
        {
            _dbContext = dbContext;
            _currentLanguageService = currentLanguageService;
            _mapper = mapper;
        }

        public async Task<RoomDetailsVM> Handle(FindRoomBySlugQuery request, CancellationToken cancellationToken)
        {
            var room = await _dbContext.Rooms
                .Include(x => x.Amenities)
                    .ThenInclude(x => x.Translations.Where(z => z.LangCode == _currentLanguageService.LangCode))
                .Include(x => x.Requirements)
                    .ThenInclude(x => x.Translations.Where(z => z.LangCode == _currentLanguageService.LangCode))
                .Include(x => x.Category)
                    .ThenInclude(x => x.Translations.Where(z => z.LangCode == _currentLanguageService.LangCode))
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Slug == request.Slug, cancellationToken);

            if (room is null)
            {
                throw new NotFoundException(nameof(Room), request.Slug);
            }

            return _mapper.Map<RoomDetailsVM>(room);
        }
    }
}
