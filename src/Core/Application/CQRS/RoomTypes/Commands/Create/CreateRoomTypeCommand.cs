using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.CQRS.RoomTypes.Queries.Search;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.RoomTypes.Commands.Create
{
    public class CreateRoomTypeCommand : IRequest<Unit>
    {
        public string Icon { get; set; }
        public List<RoomTypeTranslationVM> Translations { get; set; }

        public class Handler : IRequestHandler<CreateRoomTypeCommand, Unit>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(CreateRoomTypeCommand request, CancellationToken cancellationToken)
            {

                var roomType = new RoomType
                {
                    Icon = request.Icon,
                    Translations = request.Translations
                        .Where(x => !string.IsNullOrEmpty(x.Name))
                        .Select(x => new RoomTypeTranslation
                        {
                            Name = x.Name,
                            LangCode = x.LangCode
                        })
                        .ToList()
                };

                _dbContext.RoomTypes.Add(roomType);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
