using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.RoomTypes.Commands.AddOrUpdateTranslation
{
    public class AddOrUpdateRoomTypeTranslationCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string LangCode { get; set; }
        public string Name { get; set; }

        public class Handler : IRequestHandler<AddOrUpdateRoomTypeTranslationCommand, bool>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(AddOrUpdateRoomTypeTranslationCommand request, CancellationToken cancellationToken)
            {
                var roomType = await _dbContext.RoomTypes
                    .Include(x => x.Translations)
                    .FirstOrDefaultAsync(
                        x => !x.Deleted && x.Id == request.Id,
                    cancellationToken);

                if (roomType is null)
                {
                    throw new NotFoundException(nameof(RoomType), request.Id);
                }

                request.Name = request.Name.Trim();
                var translation = roomType.Translations.FirstOrDefault(x => x.LangCode == request.LangCode);
                if (translation is not null)
                {
                    translation.Name = request.Name;
                }
                else
                {
                    roomType.Translations.Add(new RoomTypeTranslation
                    {
                        LangCode = request.LangCode,
                        Name = request.Name
                    });
                }


                _dbContext.RoomTypes.Update(roomType);
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}
