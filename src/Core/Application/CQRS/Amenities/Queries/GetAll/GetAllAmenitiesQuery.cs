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
    public class GetAllAmenitiesQuery : IRequest<List<AmenitieDetailsVM>>
    {
        public class Handler : IRequestHandler<GetAllAmenitiesQuery, List<AmenitieDetailsVM>>
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

            public Task<List<AmenitieDetailsVM>> Handle(GetAllAmenitiesQuery request, CancellationToken cancellationToken)
            {
                return _dbContext.Amenities
                    .Include(x => x.Translations.Where(x => x.LangCode == _currentLanguageService.LangCode))
                    .ProjectTo<AmenitieDetailsVM>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }
    }

}
