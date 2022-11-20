using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
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

        public async Task<UserDTO> Handle(FindUserByEmailQuery request, CancellationToken cancellationToken)
        {
            var res = await _userManager.FindByEmailAsync(request.Email, tenantInfo.Domain);

            if (!res.Result.Succeeded)
            {
                throw new NotFoundException("User", request.Email);
            }

            return res.User;
        }
    }
}


