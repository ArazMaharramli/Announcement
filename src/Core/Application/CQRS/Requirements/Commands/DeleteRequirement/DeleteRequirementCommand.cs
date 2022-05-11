using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Requirements.Commands.DeleteRequirement
{
    public class DeleteRequirementCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public class Handler : IRequestHandler<DeleteRequirementCommand, bool>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(DeleteRequirementCommand request, CancellationToken cancellationToken)
            {
                var requirement = await _dbContext.Requirements.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                _dbContext.Requirements.Remove(requirement);

                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}
