using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.CQRS.Users.CreateUser
{
    public class CreateUserCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string UserType { get; set; }


        public class Handler : IRequestHandler<CreateUserCommand, string>
        {
            private readonly IUserManager _userManager;
            private readonly IMediator _mediator;

            public Handler(IUserManager userManager, IMediator mediator)
            {
                _userManager = userManager;
                _mediator = mediator;
            }



            public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                var response = await _userManager.CreateUserAsync(userName: request.Email, password: request.Password);
                if (!response.Result.Succeeded)
                {
                    throw new ValidationException(response.Result.Errors.Select(x => new FluentValidation.Results.ValidationFailure("", x)).ToList());
                }
                await _mediator.Publish(new SendEmailConfirmationCode { UserId = response.UserId, UserType = request.UserType }, cancellationToken);
                return response.UserId;
            }
        }
    }
}
