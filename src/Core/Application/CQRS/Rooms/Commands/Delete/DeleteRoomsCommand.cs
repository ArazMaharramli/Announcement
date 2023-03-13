using System;
using Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace Application.CQRS.Rooms.Commands.Delete;

public class DeleteRoomsCommand : IRequest<Unit>
{
    public string[] Ids { get; set; }
    public class Handler : IRequestHandler<DeleteRoomsCommand, Unit>
    {
        private readonly IDbContext _dbContext;

        public Handler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteRoomsCommand request, CancellationToken cancellationToken)
        {
            var rooms = await _dbContext.Rooms.Where(x => request.Ids.Contains(x.Id)).ToListAsync(cancellationToken);
            _dbContext.Rooms.RemoveRange(rooms);

            await _dbContext.SaveEntitiesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}

