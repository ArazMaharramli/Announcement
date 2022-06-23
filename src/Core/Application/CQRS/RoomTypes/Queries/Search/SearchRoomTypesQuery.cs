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

namespace Application.CQRS.RoomTypes.Queries.Search
{
    public class SearchRoomTypesQuery : IRequest<IDataTablePagedList<RoomTypeVm>>
    {
        public bool Deleted { get; set; } = false;

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortColumn { get; set; }
        public string SortColumnDirection { get; set; } = "asc";
        public string SearchValue { get; set; }

        public string LangCode { get; set; }

        public class Handler : IRequestHandler<SearchRoomTypesQuery, IDataTablePagedList<RoomTypeVm>>
        {
            private readonly IDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(IDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<IDataTablePagedList<RoomTypeVm>> Handle(SearchRoomTypesQuery request, CancellationToken cancellationToken)
            {
                request.SortColumn = string.IsNullOrEmpty(request.SortColumn) ? nameof(RoomTypeVm.UpdatedAt) : request.SortColumn;
                request.SortColumnDirection = string.IsNullOrEmpty(request.SortColumnDirection) ? "asc" : request.SortColumnDirection;


                var list = _dbContext.RoomTypes
                    .Include(x => x.Translations)
                    .IgnoreQueryFilters()
                    .Where(x => x.Deleted == request.Deleted)
                    .ProjectTo<RoomTypeVm>(_mapper.ConfigurationProvider, new { lang = request.LangCode, deleted = request.Deleted });


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

                return new DataTablePagedList<RoomTypeVm>
                {
                    Data = data,
                    RecordsFiltered = data.Count,
                    RecordsTotal = totalCount,
                };


            }
        }
    }
}
