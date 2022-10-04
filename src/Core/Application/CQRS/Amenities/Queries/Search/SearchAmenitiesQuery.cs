using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Extentions;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Amenities.Queries.Search
{
    public class SearchAmenitiesQuery : IRequest<IDataTablePagedList<AmenitieVm>>
    {
        public bool Deleted { get; set; } = false;

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortColumn { get; set; }
        public string SortColumnDirection { get; set; } = "asc";
        public string SearchValue { get; set; }

        public class Handler : IRequestHandler<SearchAmenitiesQuery, IDataTablePagedList<AmenitieVm>>
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

            public async Task<IDataTablePagedList<AmenitieVm>> Handle(SearchAmenitiesQuery request, CancellationToken cancellationToken)
            {
                request.SortColumn = string.IsNullOrEmpty(request.SortColumn) ? nameof(AmenitieVm.UpdatedAt) : request.SortColumn;
                request.SortColumnDirection = string.IsNullOrEmpty(request.SortColumnDirection) ? "asc" : request.SortColumnDirection;


                var list = _dbContext.Amenities
                    .Include(x => x.Translations)
                    .IgnoreQueryFilters()
                    .Where(x => x.Deleted == request.Deleted)
                    .AsNoTracking()
                    .Select(x => new AmenitieVm
                    {
                        Icon = x.Icon,
                        UpdatedAt = x.UpdatedAt,
                        Id = x.Id,
                        Name = x.GetName(_currentLanguageService.LangCode),
                        Translations = x.Translations.Select(z => new AmenitieTranslationVM
                        {
                            Id = z.Id,
                            LangCode = z.LangCode,
                            Name = z.Name
                        }).ToList()
                    });


                if (request.SortColumnDirection == "asc")
                {
                    list = list.OrderBy(request.SortColumn);
                }
                else if (request.SortColumnDirection == "desc")
                {
                    list = list.OrderByDescending(request.SortColumn);
                }


                //search
                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    list = list.Where(m =>
                            m.Translations.Any(x => x.Name.Contains(request.SearchValue)) ||
                            m.UpdatedAt.ToString().Contains(request.SearchValue)
                        );
                }

                var totalCount = list.Count();
                var data = await list
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                return new DataTablePagedList<AmenitieVm>
                {
                    Data = data,
                    RecordsFiltered = data.Count,
                    RecordsTotal = totalCount,
                };


            }
        }
    }
}
