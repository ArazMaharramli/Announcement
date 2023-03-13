using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CQRS.Rooms.IntegrationEvents.Events;
using Domain.Common;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Rooms.Commands.UpdateByOwner;

public class UpdateRoomByOwnerCommand : IRequest<Unit>
{
    public string Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public int Price { get; set; }

    public string AddressLine { get; set; }
    //public double Lat { get; set; }
    //public double Lng { get; set; }

    public Contact Contact { get; set; }

    public string CategoryId { get; set; }

    public List<string> AmenitieIds { get; set; }
    public List<string> RequirementIds { get; set; }
    public List<string> Medias { get; set; }

    public class Handler : IRequestHandler<UpdateRoomByOwnerCommand, Unit>
    {
        private readonly IDbContext _dbContext;
        private readonly IEventBusService _eventBusService;

        public Handler(IDbContext dbContext, IEventBusService eventBusService)
        {
            _dbContext = dbContext;
            _eventBusService = eventBusService;
        }

        public async Task<Unit> Handle(UpdateRoomByOwnerCommand request, CancellationToken cancellationToken)
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

            room.Title = request.Title.Trim();

            room.Address.AddressLine = request.AddressLine;
            //= new Address(request.AddressLine, request.Lng, request.Lat);

            room.CategoryId = request.CategoryId;
            room.Contact = request.Contact;
            room.Description = request.Description;

            room.UpdateMedias(request.Medias);

            room.Price = request.Price;
            room.Requirements = requirements;
            room.Amenities = amenities;

            room.Update();

            _dbContext.Rooms.Update(room);
            await _dbContext.SaveEntitiesAsync(cancellationToken);

            _eventBusService.AddEvent(new RoomUpdatedByOwnerIntegrationEvent(room));
            return Unit.Value;
        }
    }
}