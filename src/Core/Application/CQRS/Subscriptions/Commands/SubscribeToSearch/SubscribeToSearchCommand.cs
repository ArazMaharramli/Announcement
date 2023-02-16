using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Subscriptions.Commands.SubscribeToSearch;

public class SubscribeToSearchCommand : IRequest<Unit>
{
    public string SearchBox { get; set; }
    public string Query { get; set; }
    public double? Lat { get; set; }
    public double? Lng { get; set; }

    public string CategoryId { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public class Handler : IRequestHandler<SubscribeToSearchCommand, Unit>
    {
        private readonly IDbContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IDbContext dbContext, ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(SubscribeToSearchCommand request, CancellationToken cancellationToken)
        {
            var search = new Search(
                    searchBox: request.SearchBox,
                    query: request.Query,
                    categoryId: request.CategoryId,
                    lat: request.Lat,
                    lng: request.Lng
                );

            var existingSearch = await _dbContext.Searches
                .Include(x => x.Subscribers)
                .FirstOrDefaultAsync(x =>
                    x.CategoryId == search.CategoryId
                    && x.SearchBox == search.SearchBox
                    && x.Query == search.Query
                    && ((x.Location == null && search.Location == null) || x.Location.Distance(search.Location) <= 300),
                    cancellationToken);

            var subscriber = await _dbContext.Subscribers.FirstOrDefaultAsync(x =>
                x.UserId == _currentUserService.UserId
                || x.Email == request.Email,
                cancellationToken);

            if (subscriber is null)
            {
                subscriber = new Subscriber(userId: _currentUserService.UserId,
                    name: request.Name,
                    email: request.Email,
                    phone: request.Phone,
                    subscribedToMarketing: false);
            }

            if (existingSearch is not null)
            {
                if (!existingSearch.Subscribers.Any(x => x.UserId == _currentUserService.UserId || x.Email == request.Email))
                {
                    existingSearch.AddSubscriber(subscriber);
                    _dbContext.Searches.Update(existingSearch);
                }
            }
            else
            {
                search.AddSubscriber(subscriber);
                _dbContext.Searches.Add(search);
            }

            await _dbContext.SaveEntitiesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}

