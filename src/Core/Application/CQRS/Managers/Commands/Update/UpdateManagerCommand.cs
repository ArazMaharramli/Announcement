using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.CQRS.Managers.IntegrationEvents.ManagerRolesUpdated;
using Application.CQRS.Managers.IntegrationEvents.ManagerUpdated;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Managers.Commands.Update;

public class UpdateManagerCommand : IRequest<Unit>
{
    public string Id { get; set; }
    public string Name { get; set; }

    public string Phone { get; set; }

    public List<string> RoleIds { get; set; }

    public class Handler : IRequestHandler<UpdateManagerCommand, Unit>
    {
        private readonly IDbContext _dbContext;
        private readonly IEventBusService _eventBusService;

        public Handler(IDbContext dbContext, IEventBusService eventBusService)
        {
            _dbContext = dbContext;
            _eventBusService = eventBusService;
        }

        public async Task<Unit> Handle(UpdateManagerCommand request, CancellationToken cancellationToken)
        {
            var manager = await _dbContext.Managers
                .Include(x => x.Roles.Where(x => !x.Deleted))
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (manager is null)
            {
                throw new NotFoundException(nameof(Manager), request.Id);
            }

            //if (manager.Email != request.Email)
            //{
            //    add integration event to send email for changing to new email
            //    manager.Email = request.Email;
            //}

            manager.Name = request.Name.Trim();
            manager.Phone = request.Phone.Trim();

            manager.UpdateRoles(await _dbContext.Roles.Where(x => request.RoleIds.Contains(x.Id)).ToListAsync(cancellationToken));
            _eventBusService.AddEvent(new ManagerRolesUpdatedIntegrationEvent(manager.Id, manager.Roles.Select(x => x.Name).ToList()));

            _dbContext.Managers.Update(manager);
            await _dbContext.SaveEntitiesAsync(cancellationToken);

            _eventBusService.AddEvent(new ManagerUpdatedIntegrationEvent(manager.Id, manager.Name, manager.Email, manager.Phone));

            return Unit.Value;
        }
    }
}