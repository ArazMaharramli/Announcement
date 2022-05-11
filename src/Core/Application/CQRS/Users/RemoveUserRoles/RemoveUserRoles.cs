using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.CQRS.Users.RemoveUserRoles
{
    public class RemoveUserRoles : INotification
    {
        public string UserId { get; set; }

        public class Handler : INotificationHandler<RemoveUserRoles>
        {
            private readonly IUserManager _userManager;

            public Handler(IUserManager userManager)
            {
                _userManager = userManager;
            }

            public Task Handle(RemoveUserRoles request, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}
