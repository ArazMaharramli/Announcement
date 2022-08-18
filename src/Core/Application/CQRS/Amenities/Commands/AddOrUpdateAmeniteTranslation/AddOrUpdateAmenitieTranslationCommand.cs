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

namespace Application.CQRS.Amenities.Commands.AddOrUpdateAmenitieTranslation
{
    public class AddOrUpdateAmenitieTranslationCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string LangCode { get; set; }
        public string Name { get; set; }

        public class Handler : IRequestHandler<AddOrUpdateAmenitieTranslationCommand, bool>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(AddOrUpdateAmenitieTranslationCommand request, CancellationToken cancellationToken)
            {
                var amenitie = await _dbContext.Amenities
                    .Include(x => x.Translations)
                    .FirstOrDefaultAsync(
                        x => !x.Deleted && x.Id == request.Id,
                    cancellationToken);

                if (amenitie is null)
                {
                    throw new NotFoundException(nameof(Amenitie), request.Id);
                }

                request.Name = request.Name.Trim();
                var translation = amenitie.Translations.FirstOrDefault(x => x.LangCode == request.LangCode);
                if (translation is not null)
                {
                    translation.Name = request.Name;
                }
                else
                {
                    amenitie.Translations.Add(new AmenitieTranslation
                    {
                        LangCode = request.LangCode,
                        Name = request.Name
                    });
                }


                _dbContext.Amenities.Update(amenitie);
                return await _dbContext.SaveEntitiesAsync(cancellationToken);
            }
        }
    }
}
