using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.CQRS.Roles.IntegrationEvents.RoleUpdated;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Roles.Commands.Update
{
    public class UpdateRoleCommand : IRequest<Unit>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Claims { get; set; }

        public class Handler : IRequestHandler<UpdateRoleCommand, Unit>
        {
            private readonly IDbContext _dbContext;
            private readonly IEventBusService _eventBusService;

            public Handler(IDbContext dbContext, IEventBusService eventBusService)
            {
                _dbContext = dbContext;
                _eventBusService = eventBusService;
            }

            public async Task<Unit> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
            {
                request.Name = request.Name.Trim();

                var role = await _dbContext.Roles
                    .Include(x => x.Claims.Where(y => !y.Deleted))
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (role is null)
                {
                    throw new NotFoundException(nameof(Role), request.Id);
                }

                role.Update(request.Name, request.Claims);

                _dbContext.Roles.Update(role);
                await _dbContext.SaveEntitiesAsync(cancellationToken);

                _eventBusService.AddEvent(new RoleUpdatedIntegrationEvent(role.Id, role.Name, role.Claims.Select(x => x.ClaimName).ToList()));
                return Unit.Value;
            }
        }

    }
}

