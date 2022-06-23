using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.CQRS.Amenities.Queries.Search;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Amenities.Commands.Create
{
    public class CreateAmenitieCommand : IRequest<Unit>
    {
        public string Icon { get; set; }
        public List<AmenitieTranslationVM> Translations { get; set; }

        public class Handler : IRequestHandler<CreateAmenitieCommand, Unit>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(CreateAmenitieCommand request, CancellationToken cancellationToken)
            {

                var amenitie = new Amenitie
                {
                    Icon = request.Icon,
                    Translations = request.Translations
                    .Where(x => !string.IsNullOrEmpty(x.Name))
                    .Select(x => new AmenitieTranslation
                    {
                        Name = x.Name,
                        LangCode = x.LangCode
                    })
                    .ToList()
                };

                _dbContext.Amenities.Add(amenitie);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
