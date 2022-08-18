using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Amenities.Commands.Delete
{
    public class DeleteAmenitiesCommand : IRequest<bool>
    {
        public string[] Ids { get; set; }
        public class Handler : IRequestHandler<DeleteAmenitiesCommand, bool>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(DeleteAmenitiesCommand request, CancellationToken cancellationToken)
            {
                var amenities = await _dbContext.Amenities.Where(x => request.Ids.Contains(x.Id)).ToListAsync(cancellationToken);
                _dbContext.Amenities.RemoveRange(amenities);

                return await _dbContext.SaveEntitiesAsync(cancellationToken);
            }
        }
    }
}
