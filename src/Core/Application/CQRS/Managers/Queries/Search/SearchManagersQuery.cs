using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Extentions;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Managers.Queries.Search;

public class SearchManagersQuery : IRequest<IDataTablePagedList<ManagerIndexModel>>
{
    public int Skip { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public string SortColumn { get; set; }
    public string SortColumnDirection { get; set; } = "asc";
    public string SearchValue { get; set; }
    public bool Deleted { get; set; } = false;

    public class Handler : IRequestHandler<SearchManagersQuery, IDataTablePagedList<ManagerIndexModel>>
    {
        private readonly IDbContext _dbContext;

        public Handler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IDataTablePagedList<ManagerIndexModel>> Handle(SearchManagersQuery request, CancellationToken cancellationToken)
        {
            var list = _dbContext.Managers
               .Where(x => x.Deleted == request.Deleted)
               .Select(x => new ManagerIndexModel
               {
                   Id = x.Id,
                   Name = x.Name,
                   CreatedAt = x.CreatedAt,
               });

            var recordTotal = list.Count();

            //Sorting
            if (!(string.IsNullOrEmpty(request.SortColumn) || string.IsNullOrEmpty(request.SortColumnDirection)))
            {
                if (request.SortColumnDirection == "asc")
                {
                    list = list.OrderBy(request.SortColumn);
                }
                else if (request.SortColumnDirection == "desc")
                {
                    list = list.OrderByDescending(request.SortColumn);
                }
            }

            //search
            if (!string.IsNullOrEmpty(request.SearchValue))
            {
                list = list.Where(m =>
                        m.Name.Contains(request.SearchValue) ||
                        m.CreatedAt.ToString().Contains(request.SearchValue)
                    );
            }


            var recordsFiltered = list.Count();

            var data = await list
                .Skip(request.Skip)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new DataTablePagedList<ManagerIndexModel>(
                data: data,
                recordsTotal: recordTotal,
                recordsFiltered: recordsFiltered);
        }
    }
}
