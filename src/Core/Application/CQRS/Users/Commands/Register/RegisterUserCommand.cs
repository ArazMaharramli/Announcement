using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Common.Models.ConfigModels;
using Application.CQRS.Users.Events.IntegrationEvents;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Users.Commands.Register;

public class RegisterUserCommand : IRequest<UserDTO>
{
    // can be null
    public string Id { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Name { get; set; }

    public class Handler : IRequestHandler<RegisterUserCommand, UserDTO>
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

        public async Task<UserDTO> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var resp = await _userManager.CreateUserAsync(
                name: request.Name,
                userName: _tenantInfo.Domain.ToLower() + "_" + request.Email,
                phoneNumber: request.Phone,
                email: request.Email,
                id: request.Id);

            if (!resp.Result.Succeeded)
            {
                throw new BadRequestException(string.Join(';', resp.Result.Errors));
            }

            _eventBusService.AddEvent(new UserRegisteredIntegrationEvent(resp.User));
            return resp.User;
        }
    }
}

