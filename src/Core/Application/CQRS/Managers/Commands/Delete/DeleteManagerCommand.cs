using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.CQRS.Managers.IntegrationEvents.ManagerDeleted;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Managers.Commands.Delete;

public class DeleteManagerCommand : IRequest<Unit>
{
    public string[] Ids { get; set; }

    public class Handler : IRequestHandler<DeleteManagerCommand, Unit>
    {
        private readonly IDbContext _dbContext;
        private readonly IEventBusService _eventBusService;

        public Handler(IDbContext dbContext, IEventBusService eventBusService)
        {
            _dbContext = dbContext;
            _eventBusService = eventBusService;
        }

        public async Task<Unit> Handle(DeleteManagerCommand request, CancellationToken cancellationToken)
        {
            var managers = await _dbContext.Managers
                .Where(x => request.Ids.Contains(x.Id))
                .ToListAsync(cancellationToken);

            if (managers is null)
            {
                throw new NotFoundException(nameof(Manager), request.Ids);
            }

            foreach (var item in managers)
            {
                _eventBusService.AddEvent(new ManagerDeletedIntegrationEvent(item.Id));
            }

            _dbContext.Managers.RemoveRange(managers);
            await _dbContext.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
