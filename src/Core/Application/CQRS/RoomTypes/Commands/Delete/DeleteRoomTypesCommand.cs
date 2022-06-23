using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.RoomTypes.Commands.Delete
{
    public class DeleteRoomTypesCommand : IRequest<bool>
    {
        public string[] Ids { get; set; }
        public class Handler : IRequestHandler<DeleteRoomTypesCommand, bool>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(DeleteRoomTypesCommand request, CancellationToken cancellationToken)
            {
                var roomTypes = await _dbContext.RoomTypes.Where(x => request.Ids.Contains(x.Id)).ToListAsync(cancellationToken);
                _dbContext.RoomTypes.RemoveRange(roomTypes);

                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}
