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

namespace Application.CQRS.Requirements.Queries.FindByRequirementId
{
    public class FindByRequirementIdQuery : IRequest<RequirementDto>
    {
        public string Id { get; set; }
        public string LangCode { get; set; }
        public bool Deleted { get; set; }

        public class Handler : IRequestHandler<FindByRequirementIdQuery, RequirementDto>
        {
            private readonly IDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(IDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<RequirementDto> Handle(FindByRequirementIdQuery request, CancellationToken cancellationToken)
            {
                var requirement = await _dbContext.Requirements
                    .Include(x => x.Translations.Where(y => !y.Deleted && y.LangCode == request.LangCode))
                    .FirstOrDefaultAsync(x => x.Deleted == request.Deleted && x.Id == request.Id);

                return _mapper.Map<RequirementDto>(requirement);
            }
        }
    }
}
