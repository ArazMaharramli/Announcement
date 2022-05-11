using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Amenities.Commands.DeleteAmenitie
{
    public class DeleteAmenitieCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public class Handler : IRequestHandler<DeleteAmenitieCommand, bool>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(DeleteAmenitieCommand request, CancellationToken cancellationToken)
            {
                var amenitie = await _dbContext.Amenities.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                _dbContext.Amenities.Remove(amenitie);

                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}
