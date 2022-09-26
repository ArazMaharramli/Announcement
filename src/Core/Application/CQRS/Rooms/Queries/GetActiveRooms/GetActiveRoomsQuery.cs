using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models.ConfigModels;
using Application.DTOS;
using Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Rooms.Queries.GetActiveRooms
{
    public class GetActiveRoomsQuery : IRequest<InfinityScroolList<RoomBriefVM>>
    {
        public string Query { get; set; }
        public string CategoryId { get; set; }
        public DateTime? StartFrom { get; set; }

        public class Handler : IRequestHandler<GetActiveRoomsQuery, InfinityScroolList<RoomBriefVM>>
        {
            private readonly IDbContext _dbContext;
            private readonly ICurrentLanguageService _currentLanguageService;
            private readonly StaticUrls _staticUrls;

            public Handler(IDbContext dbContext, ICurrentLanguageService currentLanguageService, StaticUrls staticUrls)
            {
                _dbContext = dbContext;
                _currentLanguageService = currentLanguageService;
                _staticUrls = staticUrls;
            }

            public async Task<InfinityScroolList<RoomBriefVM>> Handle(GetActiveRoomsQuery request, CancellationToken cancellationToken)
            {
                var queryIsEmpty = string.IsNullOrEmpty(request.Query?.Trim());
                var categoryIsEmpty = string.IsNullOrEmpty(request.CategoryId?.Trim());
                var query = request.Query?.Trim();

                var rooms = await _dbContext.Rooms
                    .Include(x => x.Category)
                        .ThenInclude(x => x.Translations.Where(z => z.LangCode == _currentLanguageService.LangCode))
                    .Include(x => x.Amenities)
                        .ThenInclude(x => x.Translations.Where(z => z.LangCode == _currentLanguageService.LangCode))
                    .Where(x =>
                        x.Status == Domain.Common.RoomStatus.Active
                        && (request.StartFrom == null || x.UpdatedAt <= request.StartFrom)
                        && (queryIsEmpty
                            || (x.Title.Contains(query)
                            || x.Description.Contains(query)
                            || x.Meta.Keywords.Contains(query)
                            || x.Meta.Description.Contains(query))
                        )
                        && (categoryIsEmpty || x.CategoryId == request.CategoryId)
                    )
                    .OrderBy(x => x.UpdatedAt)
                    .Take(10)
                    .Select(x => new RoomBriefVM
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Price = x.Price,
                        Url = String.Format(_staticUrls.RoomDetails, x.Slug),
                        MediaUrl = x.Medias.FirstOrDefault().Url,
                        MediaAltTag = x.Medias.FirstOrDefault().AltTag,
                    })
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                return new InfinityScroolList<RoomBriefVM>(rooms, rooms.Any());
            }
        }
    }
}

