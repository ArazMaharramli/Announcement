using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.RoomTypes.Commands.UpdateRoomTypeImage
{
    public class UpdateRoomTypeImageCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string Image { get; set; }

        public class Handler : IRequestHandler<UpdateRoomTypeImageCommand, bool>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(UpdateRoomTypeImageCommand request, CancellationToken cancellationToken)
            {
                var roomType = await _dbContext.RoomTypes.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
                roomType.Image = request.Image;

                _dbContext.RoomTypes.Update(roomType);
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}
