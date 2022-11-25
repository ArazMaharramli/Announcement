using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.CQRS.Managers.IntegrationEvents.ManagerRemovedFromRole;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Managers.Commands.RemoveManagersFromRole;

public class RemoveManagersFromRoleCommand : IRequest<Unit>
{
    public string[] Ids { get; set; }
    public string RoleName { get; set; }

    public class Handler : IRequestHandler<RemoveManagersFromRoleCommand, Unit>
    {
        private readonly IDbContext _dbContext;
        private readonly IEventBusService _eventBusService;

        public Handler(IDbContext dbContext, IEventBusService eventBusService)
        {
            _dbContext = dbContext;
            _eventBusService = eventBusService;
        }

        public async Task<Unit> Handle(RemoveManagersFromRoleCommand request, CancellationToken cancellationToken)
        {
            var managers = await _dbContext.Managers
                .Include(x => x.Roles)
                .Where(x => request.Ids.Contains(x.Id))
                .ToListAsync(cancellationToken);

            foreach (var manager in managers)
            {
                var role = manager.Roles.FirstOrDefault(x => x.Name.ToLower() == request.RoleName.ToLower());
                manager.Roles.Remove(role);
                _eventBusService.AddEvent(new ManagerRemovedFromRoleIntegrationEvent(manager.Id, role.Name));
            }

            _dbContext.Managers.UpdateRange(managers);
            await _dbContext.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}