using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Requirements.Queries.GetAll
{
    public class GetAllRequirementsQuery : IRequest<List<RequirementDetailsVM>>
    {
        public class Handler : IRequestHandler<GetAllRequirementsQuery, List<RequirementDetailsVM>>
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

            public Task<List<RequirementDetailsVM>> Handle(GetAllRequirementsQuery request, CancellationToken cancellationToken)
            {
                return _dbContext.Requirements
                    .Include(x => x.Translations.Where(z => z.LangCode == _currentLanguageService.LangCode))
                    .ProjectTo<RequirementDetailsVM>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
