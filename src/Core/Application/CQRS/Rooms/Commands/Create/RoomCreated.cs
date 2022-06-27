using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Rooms.Commands.Create
{
    public class RoomCreated : INotification
    {
        public Room Room { get; set; }

        public class Handler : INotificationHandler<RoomCreated>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public Task Handle(RoomCreated request, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }
    }
}

