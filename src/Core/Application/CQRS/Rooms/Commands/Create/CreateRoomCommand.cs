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
        public string ContactEmail { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public int Price { get; set; }

        public string Address { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }

        public string CategoryId { get; set; }

        public List<string> AmenitieIds { get; set; }
        public List<string> RequirementIds { get; set; }
        public List<string> MediaUrls { get; set; }

        public class Handler : IRequestHandler<CreateRoomCommand, Unit>
        {
            private readonly IDbContext _dbContext;
            private readonly IMediator _mediator;
            private readonly ICurrentLanguageService _currentLanguageService;

            public Handler(IDbContext dbContext, IMediator mediator, ICurrentLanguageService currentLanguageService)
            {
                _dbContext = dbContext;
                _mediator = mediator;
                _currentLanguageService = currentLanguageService;
            }

            public async Task<Unit> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
            {
                var amenities = await _dbContext.Amenities
                    .Where(x => request.AmenitieIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);

                var requirements = await _dbContext.Requirements
                    .Where(x => request.RequirementIds.Contains(x.Id))
                    .ToListAsync(cancellationToken);

                var category = await _dbContext.CategoryTranslations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(z => z.LangCode == _currentLanguageService.LangCode && z.CategoryId == request.CategoryId, cancellationToken);

                var slug = request.Name.ToUrlSlug();
                var slugCount = await _dbContext.Rooms.CountAsync(x => x.Slug.Contains(slug), cancellationToken);
                slug += $"-{(slugCount > 0 ? slugCount : "")}";

                var room = new Room(
                    name: request.Name.Trim(),
                    slug: slug,
                    description: request.Description.Trim(),
                    price: request.Price,
                    address: request.Address.Trim(),
                    lng: request.Lng,
                    lat: request.Lat,
                    contactPhone: request.ContactPhone.Trim(),
                    contactEmail: request.ContactEmail.Trim(),
                    contactName: request.ContactName.Trim(),
                    categoryId: request.CategoryId,
                    amenities: amenities,
                    requirements: requirements,
                    mediaUrls: request.MediaUrls,
                    metaKeywords: category.Meta.Keywords);

                _dbContext.Rooms.Add(room);
                await _dbContext.SaveEntitiesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}

