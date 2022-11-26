using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CQRS.Managers.IntegrationEvents.ManagerRecovered;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Managers.Commands.Recover;

public class RecoverManagersCommand : IRequest<Unit>
{
    public string[] Ids { get; set; }

    public class Handler : IRequestHandler<RecoverManagersCommand, Unit>
    {
        private readonly IDbContext _dbContext;
        private readonly IEventBusService _eventBusService;

        public Handler(IDbContext dbContext, IEventBusService eventBusService)
        {
            _dbContext = dbContext;
            _eventBusService = eventBusService;
        }

        public async Task<Unit> Handle(RecoverManagersCommand request, CancellationToken cancellationToken)
        {
            var deletedManagers = await _dbContext.Managers
                .Include(x => x.Roles)
                .Include(x => x.Claims)
                .IgnoreQueryFilters()
                .Where(x => x.Deleted && request.Ids.Contains(x.Id))
                .ToListAsync(cancellationToken);

            deletedManagers.ForEach(x =>
            {
                x.Recover();
                _eventBusService.AddEvent(new ManagerRecoveredIntegrationEvent(x.Id, x.Roles.Select(z => z.Name).ToList(), x.Claims.Select(z => z.ClaimName).ToList()));
            });

            _dbContext.Managers.UpdateRange(deletedManagers);

            await _dbContext.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}