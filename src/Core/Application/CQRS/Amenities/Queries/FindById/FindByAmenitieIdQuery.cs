using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOS;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Amenities.Queries.FindById
{
    public class FindByAmenitieIdQuery : IRequest<AmenitieDto>
    {
        public string Id { get; set; }

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
                    .Include(x => x.Translations)
                    .AsNoTracking()
                    .ProjectTo<AmenitieDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (amenitie is null)
                {
                    throw new NotFoundException(nameof(Amenitie), request.Id);
                }

                return amenitie;
            }
        }
    }
}
