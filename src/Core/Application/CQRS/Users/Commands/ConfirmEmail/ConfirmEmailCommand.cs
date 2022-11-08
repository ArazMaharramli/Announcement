using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;

namespace Application.CQRS.Users.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<Result>
    {
        public string UserId { get; set; }
        public string Code { get; set; }

        public class Handler : IRequestHandler<ConfirmEmailCommand, Result>
        {
            private readonly IUserManager _userManager;

            public Handler(IUserManager userManager)
            {
                _userManager = userManager;
            }

            public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
            {
                var res = await _userManager.ConfirmEmail(request.UserId, request.Code);
                return res.Result;
            }
        }
    }
}

