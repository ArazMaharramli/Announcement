using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOS;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Requirements.Commands.Update
{
    public class UpdateRequirementCommand : IRequest<Unit>
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public List<RequirementTranslationDto> Translations { get; set; }

        public class Handler : IRequestHandler<UpdateRequirementCommand, Unit>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(UpdateRequirementCommand request, CancellationToken cancellationToken)
            {
                var requirement = await _dbContext.Requirements
                    .Include(x => x.Translations)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (requirement is null)
                {
                    throw new NotFoundException(nameof(Requirement), request.Id);
                }

                requirement.Icon = request.Icon;
                requirement.Translations?.Clear();
                requirement.Translations = request.Translations.Select(x =>
                  new RequirementTranslation
                  {
                      Name = x.Name,
                      LangCode = x.LangCode
                  }).ToList();

                _dbContext.Requirements.Update(requirement);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}

