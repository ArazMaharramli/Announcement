using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Extentions;
using Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Roles.Queries.SearchUsersByRole
{
    public class SearchUsersByroleQuery : IRequest<IDataTablePagedList<UsersByRoleIndexModel>>
    {
        public int Skip { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public string SortColumn { get; set; }
        public string SortColumnDirection { get; set; } = "asc";
        public string SearchValue { get; set; }
        public bool Deleted { get; set; } = false;

        public string RoleName { get; set; }

        public class Handler : IRequestHandler<SearchUsersByroleQuery, IDataTablePagedList<UsersByRoleIndexModel>>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<IDataTablePagedList<UsersByRoleIndexModel>> Handle(SearchUsersByroleQuery request, CancellationToken cancellationToken)
            {
                var list = _dbContext.Managers
                   .Where(x => x.Deleted == request.Deleted && x.Roles.Any(y => y.Name.ToLower() == request.RoleName.ToLower()))
                   .Select(x => new UsersByRoleIndexModel
                   {
                       Id = x.UserId,
                       FullName = x.Name + " " + x.Surname,
                       CreatedAt = x.CreatedAt
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
                            m.FullName.Contains(request.SearchValue)
                        );
                }
                var recordsFiltered = list.Count();

                var data = await list.Skip(request.Skip).Take(request.PageSize).ToListAsync(cancellationToken);

                return new DataTablePagedList<UsersByRoleIndexModel>(
                    data: data,
                    recordsTotal: recordTotal,
                    recordsFiltered: recordsFiltered);
            }
        }
    }
}

