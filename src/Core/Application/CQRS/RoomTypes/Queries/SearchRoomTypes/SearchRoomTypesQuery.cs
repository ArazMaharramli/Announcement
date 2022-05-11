using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.RoomTypes.Queries.SearchRoomTypes
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
                    .Where(x => x.Deleted == request.Deleted)
                    .ProjectTo<RoomTypeVm>(_mapper.ConfigurationProvider, new { lang = request.LangCode, deleted = request.Deleted });


                if (request.SortColumnDirection == "asc")
                {
                    switch (request.SortColumn)
                    {
                        case nameof(RoomTypeVm.Name):
                            list = list.OrderBy(p => p.Name);
                            break;
                        case nameof(RoomTypeVm.UpdatedAt):
                            list = list.OrderBy(p => p.UpdatedAt);
                            break;
                        default:
                            list = list.OrderBy(p => p.UpdatedAt);
                            break;
                    }
                }
                else if (request.SortColumnDirection == "desc")
                {
                    switch (request.SortColumn)
                    {
                        case nameof(RoomTypeVm.Name):
                            list = list.OrderByDescending(p => p.Name);
                            break;
                        case nameof(RoomType.UpdatedAt):
                            list = list.OrderByDescending(p => p.UpdatedAt);
                            break;
                        default:
                            list = list.OrderByDescending(p => p.UpdatedAt);
                            break;
                    }
                }


                //search
                if (!string.IsNullOrEmpty(request.SearchValue))
                {
                    list = list.Where(m =>
                            m.Name.Contains(request.SearchValue) ||
                            m.UpdatedAt.ToString().Contains(request.SearchValue)
                        );
                }

                var totalCount = list.Count();
                var data = await list
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                return new DataTablePagedList<RoomTypeVm>(
                    data: data,
                    total: totalCount,
                    page: request.Page,
                    perpage: request.PageSize,
                    sortColumn: request.SortColumn,
                    sortDir: request.SortColumnDirection
                    );

            }
        }
    }
}
