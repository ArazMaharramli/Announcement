using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models.ConfigModels;
using Application.CQRS.Managers.IntegrationEvents.ManagerCreated;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Managers.Commands.Create;

public class CreateManagerCommand : IRequest<Unit>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public List<string> RoleIds { get; set; }

    public class Handler : IRequestHandler<CreateManagerCommand, Unit>
    {
        private readonly IDbContext _dbContext;
        private readonly IMediator _mediator;
        private readonly TenantInfo _tenantInfo;
        private readonly IUserManager _userManager;
        private readonly IEventBusService _eventBusService;

        public Handler(IDbContext dbContext, IMediator mediator, TenantInfo tenantInfo, IUserManager userManager, IEventBusService eventBusService)
        {
            _dbContext = dbContext;
            _mediator = mediator;
            _tenantInfo = tenantInfo;
            _userManager = userManager;
            _eventBusService = eventBusService;
        }

        public async Task<Unit> Handle(CreateManagerCommand request, CancellationToken cancellationToken)
        {
            var manager = new Manager(
               name: request.Name.Trim(),
               email: request.Email,
               phone: request.Phone
           );

            var userRes = await _userManager.FindByEmailAsync(request.Email, _tenantInfo.Domain);

            if (userRes.Result.Succeeded)
            {
                manager.Id = userRes.User.Id;
            }

            var rolesInDb = await _dbContext.Roles.Where(x => request.RoleIds.Contains(x.Id)).ToListAsync(cancellationToken);
            manager.AddRoleRange(rolesInDb);

            _dbContext.Managers.Add(manager);
            await _dbContext.SaveEntitiesAsync(cancellationToken);

            _eventBusService.AddEvent(new ManagerCreatedIntegrationEvent(
                        id: manager.Id,
                        name: manager.Name,
                        email: manager.Email,
                        phone: manager.Phone,
                        createUser: !userRes.Result.Succeeded,
                        roleNames: manager.Roles.Select(x => x.Name).ToList()
                    )
                );

            return Unit.Value;
        }
    }
}
