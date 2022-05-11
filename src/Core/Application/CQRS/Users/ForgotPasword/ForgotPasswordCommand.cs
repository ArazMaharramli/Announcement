using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.CQRS.Users.ForgotPasword
{
    public class ForgotPasswordCommand : IRequest<Unit>
    {
        public string Email { get; set; }

        public class Handler : IRequestHandler<ForgotPasswordCommand, Unit>
        {
            private readonly IUserManager _userManager;
            private readonly IMediator _mediator;

            public Handler(IUserManager userManager, IMediator mediator)
            {
                _userManager = userManager;
                _mediator = mediator;
            }

            public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
                await _mediator.Publish(new SendForgotPasswordEmail { UserId = user.Id, Code = token, Email = user.Email }, cancellationToken);
                return Unit.Value;
            }
        }
    }
}
