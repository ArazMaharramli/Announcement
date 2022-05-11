using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.CQRS.Users.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<bool>
    {
        public string UserId { get; set; }
        public string Token { get; set; }

        public class Handler : IRequestHandler<ConfirmEmailCommand, bool>
        {
            private readonly IUserManager _userManager;

            public Handler(IUserManager userManager)
            {
                _userManager = userManager;
            }

            public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
            {
                var response = await _userManager.ConfirmEmail(request.UserId, request.Token);
                if (!response.Result.Succeeded)
                {
                    throw new BadRequestException(response.Result.Errors[0]);
                }
                return true;
            }

        }
    }
}
