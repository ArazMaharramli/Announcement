using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;

namespace Application.CQRS.Roles.Queries.GetAllClaims
{
    public class GetAllClaimsQuery : IRequest<List<SystemClaimsVM>>
    {
        public class Handler : IRequestHandler<GetAllClaimsQuery, List<SystemClaimsVM>>
        {
            public async Task<List<SystemClaimsVM>> Handle(GetAllClaimsQuery request, CancellationToken cancellationToken)
            {

                return SystemClaims.GetSystemClaims();
            }

        }
    }
}

