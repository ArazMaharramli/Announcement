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

namespace Application.CQRS.Categories.Queries.GetAll
{
    public class GetAllCategoriesQuery : IRequest<List<CategoryDetailsVM>>
    {
        public class Handler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDetailsVM>>
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

            public Task<List<CategoryDetailsVM>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
            {
                return _dbContext.Categories
                    .Include(x => x.Translations.Where(x => x.LangCode == _currentLanguageService.LangCode))
                    .ProjectTo<CategoryDetailsVM>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            }
        }
    }
}
