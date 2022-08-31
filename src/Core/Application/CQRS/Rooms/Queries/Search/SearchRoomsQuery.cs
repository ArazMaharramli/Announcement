using System;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CQRS.Rooms.Queries.Search;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Application.Common.Extentions;

namespace Application.CQRS.Rooms.Queries.Search
{
    public class SearchRoomsQuery : IRequest<IDataTablePagedList<RoomVm>>
    {
        public bool Deleted { get; set; } = false;

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortColumn { get; set; }
        public string SortColumnDirection { get; set; } = "asc";
        public string SearchValue { get; set; }

        public string CategoryId { get; set; }

        public class Handler : IRequestHandler<SearchRoomsQuery, IDataTablePagedList<RoomVm>>
        {
            private readonly IDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(IDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<IDataTablePagedList<RoomVm>> Handle(SearchRoomsQuery request, CancellationToken cancellationToken)
            {
                request.SortColumn = string.IsNullOrEmpty(request.SortColumn) ? nameof(RoomVm.UpdatedAt) : request.SortColumn;
                request.SortColumnDirection = string.IsNullOrEmpty(request.SortColumnDirection) ? "asc" : request.SortColumnDirection;

                var emptyCategoryId = string.IsNullOrEmpty(request.CategoryId);

                var list = _dbContext.Rooms
                    .IgnoreQueryFilters()
                    .Where(x => x.Deleted == request.Deleted && (emptyCategoryId || x.CategoryId == request.CategoryId))
                    .AsNoTracking()
                    .ProjectTo<RoomVm>(_mapper.ConfigurationProvider);


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
                            m.Name.Contains(request.SearchValue) ||
                            m.UpdatedAt.ToString().Contains(request.SearchValue)
                        );
                }

                var totalCount = list.Count();
                var data = await list
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                return new DataTablePagedList<RoomVm>
                {
                    Data = data,
                    RecordsFiltered = data.Count,
                    RecordsTotal = totalCount,
                };
            }
        }
    }
}

