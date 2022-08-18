using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Requirements.Commands.Delete
{
    public class DeleteRequirementsCommand : IRequest<bool>
    {
        public string[] Ids { get; set; }
        public class Handler : IRequestHandler<DeleteRequirementsCommand, bool>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(DeleteRequirementsCommand request, CancellationToken cancellationToken)
            {
                var requirements = await _dbContext.Requirements.Where(x => request.Ids.Contains(x.Id)).ToListAsync(cancellationToken);
                _dbContext.Requirements.RemoveRange(requirements);

                return await _dbContext.SaveEntitiesAsync(cancellationToken);
            }
        }
    }
}
