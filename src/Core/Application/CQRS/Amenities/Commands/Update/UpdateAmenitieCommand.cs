using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.CQRS.Amenities.Commands.Create;
using Application.DTOS;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Amenities.Commands.Update
{
    public class UpdateAmenitieCommand : IRequest<Unit>
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public List<AmenitieTranslationDto> Translations { get; set; }

        public class Handler : IRequestHandler<UpdateAmenitieCommand, Unit>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(UpdateAmenitieCommand request, CancellationToken cancellationToken)
            {
                var amenitie = await _dbContext.Amenities
                    .Include(x => x.Translations)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (amenitie is null)
                {
                    throw new NotFoundException(nameof(Amenitie), request.Id);
                }

                amenitie.Icon = request.Icon;
                amenitie.Translations?.Clear();
                amenitie.Translations = request.Translations.Select(x =>
                  new AmenitieTranslation
                  {
                      Name = x.Name,
                      LangCode = x.LangCode
                  }).ToList();

                _dbContext.Amenities.Update(amenitie);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}

