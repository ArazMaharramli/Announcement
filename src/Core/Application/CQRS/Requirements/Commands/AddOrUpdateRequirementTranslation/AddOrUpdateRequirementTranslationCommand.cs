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

namespace Application.CQRS.Requirements.Commands.AddOrUpdateRequirementTranslation
{
    public class AddOrUpdateRequirementTranslationCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string LangCode { get; set; }
        public string Name { get; set; }

        public class Handler : IRequestHandler<AddOrUpdateRequirementTranslationCommand, bool>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(AddOrUpdateRequirementTranslationCommand request, CancellationToken cancellationToken)
            {
                var requirement = await _dbContext.Requirements
                    .Include(x => x.Translations
                    .Where(y => !y.Deleted))
                    .FirstOrDefaultAsync(
                        x => !x.Deleted && x.Id == request.Id,
                    cancellationToken);

                if (requirement is null)
                {
                    throw new NotFoundException(nameof(Requirement), request.Id);
                }

                request.Name = request.Name.Trim();
                var translation = requirement.Translations.FirstOrDefault(x => !x.Deleted && x.LangCode == request.LangCode);
                if (translation is not null)
                {
                    translation.Name = request.Name;
                }
                else
                {
                    requirement.Translations.Add(new RequirementTranslation
                    {
                        LangCode = request.LangCode,
                        Name = request.Name
                    });
                }


                _dbContext.Requirements.Update(requirement);
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}
