using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOS;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Amenities.Queries.FindByAmenitieId
{
    public class FindByAmenitieIdQuery : IRequest<AmenitieDto>
    {
        public string Id { get; set; }
        public string LangCode { get; set; }

        public class Handler : IRequestHandler<FindByAmenitieIdQuery, AmenitieDto>
        {
            private readonly IDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(IDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<AmenitieDto> Handle(FindByAmenitieIdQuery request, CancellationToken cancellationToken)
            {
                var amenitie = await _dbContext.Amenities
                    .Include(x => x.Translations.Where(y => !y.Deleted))
                    .ProjectTo<AmenitieDto>(_mapper.ConfigurationProvider, new { lang = request.LangCode })
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                return amenitie;
            }
        }
    }
}
