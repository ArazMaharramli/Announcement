using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.CQRS.Roles.IntegrationEvents.RoleDeleted;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Roles.Commands.Delete;

public class DeleteRoleCommand : IRequest<Unit>
{
    public string Id { get; set; }
    public class Handler : IRequestHandler<DeleteRoleCommand, Unit>
    {
        private readonly IDbContext _dbContext;
        private readonly IEventBusService _eventBusService;

        public Handler(IDbContext dbContext, IEventBusService eventBusService)
        {
            _dbContext = dbContext;
            _eventBusService = eventBusService;
        }

        public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (role is null)
            {
                throw new NotFoundException(nameof(Role), request.Id);
            }

            _dbContext.Roles.Remove(role);
            await _dbContext.SaveEntitiesAsync(cancellationToken);
            _eventBusService.AddEvent(new RoleDeletedIntegrationEvent(role.Id));

            return Unit.Value;
        }
    }
}
