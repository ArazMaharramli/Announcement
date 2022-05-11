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

namespace Application.CQRS.Requirements.Commands.CreateRequirement
{
    public class CreateRequirementCommand : IRequest<Unit>
    {
        public string LangCode { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public class Handler : IRequestHandler<CreateRequirementCommand, Unit>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(CreateRequirementCommand request, CancellationToken cancellationToken)
            {
                var requirement = await _dbContext.Requirements
                    .Include(x => x.Translations)
                    .FirstOrDefaultAsync(
                    x =>
                        !x.Deleted &&
                        x.Translations.Any(
                            y => !y.Deleted && y.LangCode == request.LangCode &&
                            y.Name.ToLower() == request.Name.ToLower()),
                    cancellationToken);

                if (requirement is not null)
                {
                    return Unit.Value;
                }

                requirement = new Requirement
                {
                    Icon = request.Icon,
                };
                requirement.Translations.Add(new RequirementTranslation
                {
                    LangCode = request.LangCode,
                    Name = request.Name
                });

                _dbContext.Requirements.Add(requirement);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
