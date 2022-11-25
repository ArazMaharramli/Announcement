using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.CQRS.Managers.IntegrationEvents.ManagerClaimsUpdated;
using Application.CQRS.Managers.IntegrationEvents.ManagerRolesUpdated;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Managers.Commands.UpdateManagerRoles;

public class UpdateManagerRolesAndClaimsCommand : IRequest<Unit>
{
    public string Id { get; set; }
    public List<string> RoleIds { get; set; }
    public List<string> Claims { get; set; }

    public class Handler : IRequestHandler<UpdateManagerRolesAndClaimsCommand, Unit>
    {
        private readonly IDbContext _dbContext;
        private readonly IUserManager _userManager;
        private readonly IEventBusService _eventBusService;

        public Handler(IDbContext dbContext, IUserManager userManager, IEventBusService eventBusService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _eventBusService = eventBusService;
        }

        public async Task<Unit> Handle(UpdateManagerRolesAndClaimsCommand request, CancellationToken cancellationToken)
        {
            var manager = _dbContext.Managers
                .Include(x => x.Roles.Where(y => !y.Deleted))
                .ThenInclude(x => x.Claims.Where(y => !y.Deleted))
                .Include(x => x.Claims.Where(y => !y.Deleted))
                .FirstOrDefault(x => x.Id == request.Id);

            if (request.RoleIds is not null && request.RoleIds.Count > 0)
            {
                var selectedRoles = await _dbContext.Roles.Where(x => request.RoleIds.Contains(x.Id)).ToListAsync(cancellationToken);
                manager.UpdateRoles(selectedRoles);
                _eventBusService.AddEvent(new ManagerRolesUpdatedIntegrationEvent(manager.Id, selectedRoles.Select(x => x.Name).ToList()));
            }

            if (request.Claims is not null && request.Claims.Count > 0)
            {
                manager.UpdateClaims(request.Claims);
                _eventBusService.AddEvent(new ManagerClaimsUpdatedIntegrationEvent(manager.Id, request.Claims));
            }

            _dbContext.Managers.Update(manager);
            await _dbContext.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }

}

