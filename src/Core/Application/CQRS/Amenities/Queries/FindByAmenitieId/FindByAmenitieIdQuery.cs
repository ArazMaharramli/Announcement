using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOS;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Amenities.Queries.FindByAmenitieId
{
    public class FindByAmenitieIdQuery : IRequest<AmenitieDto>
    {
        public string Id { get; set; }
        public string LangCode { get; set; }
        public bool Deleted { get; set; }

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
                    .Include(x => x.Translations.Where(y => !y.Deleted && y.LangCode == request.LangCode))
                    .FirstOrDefaultAsync(x => x.Deleted == request.Deleted && x.Id == request.Id);

                return _mapper.Map<AmenitieDto>(amenitie);
            }
        }
    }
}
