using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.RoomTypes.Queries.GetAll
{
    public class GetAllRoomTypesQuery : IRequest<List<RoomTypeDetailsVM>>
    {
        public class Handler : IRequestHandler<GetAllRoomTypesQuery, List<RoomTypeDetailsVM>>
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

            public Task<List<RoomTypeDetailsVM>> Handle(GetAllRoomTypesQuery request, CancellationToken cancellationToken)
            {
                return _dbContext.RoomTypes
                    .Include(x => x.Translations)
                    .ProjectTo<RoomTypeDetailsVM>(_mapper.ConfigurationProvider, new { langCode = _currentLanguageService.LangCode })
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
