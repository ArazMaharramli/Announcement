using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.CQRS.Categories.Queries.Search;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Categories.Commands.Update
{
    public class UpdateCategoryCommand : IRequest<Unit>
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public List<CategoryTranslationVM> Translations { get; set; }

        public class Handler : IRequestHandler<UpdateCategoryCommand, Unit>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            {
                var category = await _dbContext.Categories
                    .Include(x => x.Translations)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (category is null)
                {
                    throw new NotFoundException(nameof(Category), request.Id);
                }

                category.Icon = request.Icon;
                category.Translations.Clear();
                category.Translations = request.Translations.Select(x =>
                  new CategoryTranslation
                  {
                      Name = x.Name,
                      LangCode = x.LangCode,
                      Meta = new Domain.Common.Meta
                      {
                          Title = x.MetaTitle,
                          Keywords = x.MetaKeywords,
                          Description = x.MetaDescription
                      }
                  }).ToList();

                _dbContext.Categories.Update(category);
                await _dbContext.SaveEntitiesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}

