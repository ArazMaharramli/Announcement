using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models.ConfigModels;
using Application.CQRS.Users.Events.IntegrationEvents;
using MediatR;

namespace Application.CQRS.Users.Commands.Update;

public class UpdateUserCommand : IRequest<Unit>
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }

    public class Handler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IUserManager _userManager;
        private readonly IEventBusService _eventBusService;
        private readonly TenantInfo _tenantInfo;

        public Handler(IUserManager userManager, IEventBusService eventBusService, TenantInfo tenantInfo)
        {
            _userManager = userManager;
            _eventBusService = eventBusService;
            _tenantInfo = tenantInfo;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var resp = await _userManager.UpdateUserAsync(
                name: request.Name,
                tenantDomain: _tenantInfo.Domain.ToLower(),
                phoneNumber: request.PhoneNumber,
                id: request.Id);

            if (!resp.Result.Succeeded)
            {
                throw new BadRequestException(string.Join(';', resp.Result.Errors));
            }

            _eventBusService.AddEvent(new UserUpdatedIntegrationEvent(resp.User));

            return Unit.Value;
        }
    }
}

