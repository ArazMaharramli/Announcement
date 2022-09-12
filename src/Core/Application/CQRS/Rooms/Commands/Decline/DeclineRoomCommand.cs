using System;
using Application.Common.Interfaces;
using Application.CQRS.Rooms.IntegrationEvents.Events;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Rooms.Commands.Decline;

public class DeclineRoomCommand : IRequest<Unit>
{
    public string Id { get; set; }
    public string Notes { get; set; }

    public class Handler : IRequestHandler<DeclineRoomCommand, Unit>
    {
        private readonly IDbContext _dbContext;
        private readonly IEventBusService _eventBusService;

        public Handler(IDbContext dbContext, IEventBusService eventBusService)
        {
            _dbContext = dbContext;
            _eventBusService = eventBusService;
        }

        public async Task<Unit> Handle(DeclineRoomCommand request, CancellationToken cancellationToken)
        {
            var room = await _dbContext.Rooms
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            room.Decline(request.Notes);

            await _dbContext.SaveEntitiesAsync(cancellationToken);
            _eventBusService.AddEvent(new RoomDeclinedIntegrationEvent(room, request.Notes));
            return Unit.Value;
        }
    }
}

