using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.CQRS.Roles.Queries.GetRoleClaims;

public class GetRoleClaimsQuery : IRequest<List<string>>
{
    public string Name { get; set; }

    public class Handler : IRequestHandler<GetRoleClaimsQuery, List<string>>
    {
        private readonly IRoleManager _roleManager;

        public Handler(IRoleManager roleManager)
        {
            _roleManager = roleManager;
        }

        public Task<List<string>> Handle(GetRoleClaimsQuery request, CancellationToken cancellationToken)
        {
            return _roleManager.GetClaimsByRole(request.Name);
        }

    }

}

