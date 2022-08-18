using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.CQRS.Requirements.Queries.Search;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Requirements.Commands.Create
{
    public class CreateRequirementCommand : IRequest<Unit>
    {
        public string Icon { get; set; }
        public List<RequirementTranslationVM> Translations { get; set; }

        public class Handler : IRequestHandler<CreateRequirementCommand, Unit>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(CreateRequirementCommand request, CancellationToken cancellationToken)
            {

                var requirement = new Requirement
                {
                    Icon = request.Icon,
                    Translations = request.Translations
                        .Where(x => !string.IsNullOrEmpty(x.Name))
                        .Select(x => new RequirementTranslation
                        {
                            Name = x.Name,
                            LangCode = x.LangCode
                        })
                        .ToList()
                };

                _dbContext.Requirements.Add(requirement);
                await _dbContext.SaveEntitiesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
