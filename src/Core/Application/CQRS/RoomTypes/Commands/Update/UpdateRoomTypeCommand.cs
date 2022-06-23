using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOS;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.RoomTypes.Commands.Update
{
    public class UpdateRoomTypeCommand : IRequest<Unit>
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public List<RoomTypeTranslationDto> Translations { get; set; }

        public class Handler : IRequestHandler<UpdateRoomTypeCommand, Unit>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(UpdateRoomTypeCommand request, CancellationToken cancellationToken)
            {
                var roomType = await _dbContext.RoomTypes
                    .Include(x => x.Translations)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (roomType is null)
                {
                    throw new NotFoundException(nameof(RoomType), request.Id);
                }

                roomType.Icon = request.Icon;
                roomType.Translations?.Clear();
                roomType.Translations = request.Translations.Select(x =>
                  new RoomTypeTranslation
                  {
                      Name = x.Name,
                      LangCode = x.LangCode
                  }).ToList();

                _dbContext.RoomTypes.Update(roomType);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}

