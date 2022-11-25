using System;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.DTOS;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using Application.Common.Extentions;

namespace Application.CQRS.Managers.Queries.SearchByRole;

public class SearchUsersByRoleQuery : IRequest<IDataTablePagedList<ManagerDto>>
{
    public int Skip { get; set; } = 0;
    public int PageSize { get; set; } = 10;
    public string SortColumn { get; set; }
    public string SortColumnDirection { get; set; } = "asc";
    public string SearchValue { get; set; }
    public bool Deleted { get; set; } = false;

    public string RoleName { get; set; }

    public class Handler : IRequestHandler<SearchUsersByRoleQuery, IDataTablePagedList<ManagerDto>>
    {
        private readonly IDbContext _dbContext;
        private readonly IMapper _mapper;

        public Handler(IDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IDataTablePagedList<ManagerDto>> Handle(SearchUsersByRoleQuery request, CancellationToken cancellationToken)
        {
            var list = _dbContext.Managers
                .Include(x => x.Roles)
                .IgnoreQueryFilters()
                .AsNoTracking()
               .Where(x => x.Deleted == request.Deleted && x.Roles.Any(y => y.Name.ToLower() == request.RoleName.ToLower()))
               .ProjectTo<ManagerDto>(_mapper.ConfigurationProvider);

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
                        m.Name.Contains(request.SearchValue)
                    );
            }
            var recordsFiltered = list.Count();

            var data = await list.Skip(request.Skip).Take(request.PageSize).ToListAsync(cancellationToken);

            return new DataTablePagedList<ManagerDto>(
                data: data,
                recordsTotal: recordTotal,
                recordsFiltered: recordsFiltered);
        }
    }
}
