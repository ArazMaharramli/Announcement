using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.CQRS.Amenities.Queries.GetAll;
using Application.CQRS.Requirements.Queries.GetAll;
using Application.CQRS.Rooms.IntegrationEvents.Events;
using Domain.Common;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Rooms.Commands.Update
{
    public class UpdateRoomCommand : IRequest<Unit>
    {
        public string Id { get; set; }

        public string Name { get; set; }
        public string Slug { get; set; }
        public Meta Meta { get; set; }

        public string Description { get; set; }

        public int Price { get; set; }

        public Address Address { get; set; }

        public Contact Contact { get; set; }

        public RoomStatus Status { get; set; }

        public string CategoryId { get; set; }

        public List<string> AmenitieIds { get; set; }
        public List<string> RequirementIds { get; set; }
        public List<Media> Medias { get; set; }

        public class Handler : IRequestHandler<UpdateRoomCommand, Unit>
        {
            private readonly IDbContext _dbContext;
            private readonly IEventBusService _eventBusService;

            public Handler(IDbContext dbContext, IEventBusService eventBusService)
            {
                _dbContext = dbContext;
                _eventBusService = eventBusService;
            }

            public async Task<Unit> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
            {
                var room = await _dbContext.Rooms
                    .Include(x => x.Amenities)
                    .Include(x => x.Requirements)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (room is null)
                {
                    throw new NotFoundException(nameof(Room), request.Id);
                }
                var requirements = await _dbContext.Requirements.Where(x => request.RequirementIds.Contains(x.Id)).ToListAsync(cancellationToken);
                var amenities = await _dbContext.Amenities.Where(x => request.AmenitieIds.Contains(x.Id)).ToListAsync(cancellationToken);

                room.Name = request.Name;
                room.Slug = request.Slug;
                room.Meta = request.Meta;
                room.Address = request.Address;
                room.CategoryId = request.CategoryId;
                room.Contact = request.Contact;
                room.Description = request.Description;
                room.Medias = request.Medias;
                room.Price = request.Price;
                room.Requirements = requirements;
                room.Amenities = amenities;

                _dbContext.Rooms.Update(room);
                await _dbContext.SaveEntitiesAsync(cancellationToken);

                _eventBusService.AddEvent(new RoomUpdatedIntegrationEvent(room));
                return Unit.Value;
            }
        }
    }
}

