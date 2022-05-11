using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.CQRS.Users.ResetPassword
{
    public class ResetPasswordCommand : IRequest<Unit>
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }

        public class Handler : IRequestHandler<ResetPasswordCommand, Unit>
        {
            private readonly IUserManager _userManager;

            public Handler(IUserManager userManager)
            {
                _userManager = userManager;
            }

            public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
            {
                var result = await _userManager.ResetPasswordAsync(request.UserId, request.Token, request.NewPassword);
                if (!result.Succeeded)
                {
                    throw new ValidationException(result.Errors.Select(x => new FluentValidation.Results.ValidationFailure("", x)).ToList());
                }
                return Unit.Value;
            }
        }
    }
}
