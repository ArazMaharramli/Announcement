using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.CQRS.Roles.RoleCreated.IntegrationEvents;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Roles.Commands.Create
{
    public class CreateRoleCommand : IRequest<Unit>
    {
        public string Name { get; set; }
        public List<string> Claims { get; set; }

        public class Handler : IRequestHandler<CreateRoleCommand, Unit>
        {
            private readonly IDbContext _dbContext;
            private readonly IEventBusService _eventBusService;

            public Handler(IDbContext dbContext, IEventBusService eventBusService)
            {
                _dbContext = dbContext;
                _eventBusService = eventBusService;
            }

            public async Task<Unit> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
            {
                request.Name = request.Name.Trim();

                var role = new Role(request.Name, request.Claims);

                _dbContext.Roles.Add(role);
                await _dbContext.SaveEntitiesAsync(cancellationToken);

                _eventBusService.AddEvent(new RoleCreatedIntegrationEvent(role.Id, role.Name, role.Claims.Select(x => x.ClaimName).ToList()));
                return Unit.Value;
            }
        }

    }
}

