using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOS;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Amenities.Commands.CreateAmenitie
{
    public class CreateAmenitieCommand : IRequest<Unit>
    {
        public string LangCode { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public class Handler : IRequestHandler<CreateAmenitieCommand, Unit>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(CreateAmenitieCommand request, CancellationToken cancellationToken)
            {
                var amenitie = await _dbContext.Amenities
                    .Include(x => x.Translations)
                    .FirstOrDefaultAsync(
                    x =>
                        !x.Deleted &&
                        x.Translations.Any(
                            y => !y.Deleted && y.LangCode == request.LangCode &&
                            y.Name.ToLower() == request.Name.ToLower()),
                    cancellationToken);

                if (amenitie is not null)
                {
                    return Unit.Value;
                }

                amenitie = new Amenitie
                {
                    Icon = request.Icon,
                };
                amenitie.Translations.Add(new AmenitieTranslation
                {
                    LangCode = request.LangCode,
                    Name = request.Name
                });

                _dbContext.Amenities.Add(amenitie);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
