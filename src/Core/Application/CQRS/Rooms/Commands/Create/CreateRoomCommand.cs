using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Common.Extentions;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Rooms.Commands.Create
{
    public class CreateRoomCommand : IRequest<Unit>
    {
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int Price { get; set; }

        public string Address { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }

        public string CategoryId { get; set; }
        public string RoomTypeId { get; set; }

        public List<string> AmenitieIds { get; set; }
        public List<string> RequirementIds { get; set; }
        public List<string> MediaUrls { get; set; }

        public class Handler : IRequestHandler<CreateRoomCommand, Unit>
        {
            private readonly IDbContext _dbContext;
            private readonly IMediator _mediator;

            public Handler(IDbContext dbContext, IMediator mediator)
            {
                _dbContext = dbContext;
                _mediator = mediator;
            }

            public async Task<Unit> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
            {
                var amenities = await _dbContext.Amenities
                    .Where(x => request.AmenitieIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);

                var requirements = await _dbContext.Requirements
                    .Where(x => request.RequirementIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);

                var room = new Room
                {
                    Name = request.Name,
                    Slug = request.Name.ToUrlSlug(),

                    Description = request.Description,
                    Price = request.Price,

                    Address = new Domain.Common.Address
                    {
                        AddressLine = request.Address,
                        Location = new NetTopologySuite.Geometries.Point(request.Lng, request.Lat)
                    },
                    Contact = new Domain.Common.Contact
                    {
                        Phone = request.ContactPhone,
                        Name = request.ContactName,
                    },

                    Medias = request.MediaUrls.Select(x => new Media
                    {
                        Url = x,
                        AltTag = $"{request.Name} - image",
                    }).ToList(),

                    Meta = new Domain.Common.Meta
                    {
                        Description = request.Description,
                        Title = request.Name,
                    },

                    RoomTypeId = request.RoomTypeId,
                    CategoryId = request.CategoryId,

                    Amenities = amenities,
                    Requirements = requirements,

                    Status = Domain.Common.RoomStatus.PendingConfirmation
                };

                _dbContext.Rooms.Add(room);
                await _dbContext.SaveChangesAsync(cancellationToken);
                await _mediator.Publish(new RoomCreated { Room = room }, cancellationToken);
                return Unit.Value;
            }
        }
    }
}

