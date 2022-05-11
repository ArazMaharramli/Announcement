using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.RoomTypes.Commands.CreateRoomType
{
    public class CreateRoomTypeCommand : IRequest<Unit>
    {
        public string LangCode { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }

        public class Handler : IRequestHandler<CreateRoomTypeCommand, Unit>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
            {
                var roomType = await _dbContext.RoomTypes
                    .Include(x => x.Translations)
                    .FirstOrDefaultAsync(
                    x =>
                        !x.Deleted &&
                        x.Translations.Any(
                            y => !y.Deleted && y.LangCode == request.LangCode &&
                            y.Name.ToLower() == request.Name.ToLower()),
                    cancellationToken);

                if (roomType is not null)
                {
                    return Unit.Value;
                }

                roomType = new RoomType
                {
                    Image = request.Image,
                };
                roomType.Translations.Add(new RoomTypeTranslation
                {
                    LangCode = request.LangCode,
                    Name = request.Name
                });

                _dbContext.RoomTypes.Add(roomType);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
