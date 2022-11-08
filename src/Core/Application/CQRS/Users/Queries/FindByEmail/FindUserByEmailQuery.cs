using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.ConfigModels;
using MediatR;

namespace Application.CQRS.Users.Queries.FindByEmail;

public class FindUserByEmailQuery : IRequest<UserDTO>
{
    public string Email { get; set; }

    public class Handler : IRequestHandler<FindUserByEmailQuery, UserDTO>
    {
        private readonly IUserManager _userManager;
        private readonly TenantInfo tenantInfo;

        public Handler(IUserManager userManager, TenantInfo tenantInfo)
        {
            _userManager = userManager;
            this.tenantInfo = tenantInfo;
        }

        public Task<UserDTO> Handle(FindUserByEmailQuery request, CancellationToken cancellationToken)
        {
            return _userManager.FindByEmailAsync(request.Email, tenantInfo.Domain);
        }
    }
}


