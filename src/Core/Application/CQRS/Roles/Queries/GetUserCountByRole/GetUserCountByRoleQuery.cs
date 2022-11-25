using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.CQRS.Roles.Queries.GetUserCountByRole;

public class GetUserCountByRoleQuery : IRequest<int>
{
    public string Name { get; set; }

    public class Handler : IRequestHandler<GetUserCountByRoleQuery, int>
    {
        private readonly IRoleManager _roleManager;

        public Handler(IRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        public Task<int> Handle(GetUserCountByRoleQuery request, CancellationToken cancellationToken)
        {
            return _roleManager.GetUserCountByRole(request.Name);
        }

    }

}

