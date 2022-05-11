using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.RoomTypes.Commands.DeleteRoomType
{
    public class DeleteRoomTypeCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public class Handler : IRequestHandler<DeleteRoomTypeCommand, bool>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(DeleteRoomTypeCommand request, CancellationToken cancellationToken)
            {
                var roomType = await _dbContext.RoomTypes.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                _dbContext.RoomTypes.Remove(roomType);

                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}
