using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Requirements.Commands.UpdateRequirementIcon
{
    public class UpdateRequirementIconCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string Icon { get; set; }

        public class Handler : IRequestHandler<UpdateRequirementIconCommand, bool>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(UpdateRequirementIconCommand request, CancellationToken cancellationToken)
            {
                var requirement = await _dbContext.Requirements.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                requirement.Icon = request.Icon;

                _dbContext.Requirements.Update(requirement);
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}
