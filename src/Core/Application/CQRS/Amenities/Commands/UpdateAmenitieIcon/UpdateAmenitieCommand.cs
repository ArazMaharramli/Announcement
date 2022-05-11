using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Amenities.Commands.UpdateAmenitieIcon
{
    public class UpdateAmenitieIconCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string Icon { get; set; }

        public class Handler : IRequestHandler<UpdateAmenitieIconCommand, bool>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(UpdateAmenitieIconCommand request, CancellationToken cancellationToken)
            {
                var amenitie = await _dbContext.Amenities.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                amenitie.Icon = request.Icon;

                _dbContext.Amenities.Update(amenitie);
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}
