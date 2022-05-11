using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.CQRS.Users.AddToManagerRoles
{
    public class AddUserToManagerRoles : INotification
    {
        public string UserId { get; set; }

        public class Handler : INotificationHandler<AddUserToManagerRoles>
        {
            private readonly IUserManager _userManger;

            public Handler(IUserManager userManger)
            {
                _userManger = userManger;
            }

            public Task Handle(AddUserToManagerRoles notification, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}
