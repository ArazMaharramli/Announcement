using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.CQRS.Categories.Queries.Search;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.Categories.Commands.Create
{
    public class CreateCategoryCommand : IRequest<Unit>
    {
        public string Icon { get; set; }
        public List<CategoryTranslationVM> Translations { get; set; }

        public class Handler : IRequestHandler<CreateCategoryCommand, Unit>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Unit> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {

                var category = new Category
                {
                    Icon = request.Icon,
                    Translations = request.Translations
                        .Where(x => !string.IsNullOrEmpty(x.Name))
                        .Select(x => new CategoryTranslation
                        {
                            Name = x.Name,
                            LangCode = x.LangCode,
                            Meta = new Domain.Common.Meta
                            {
                                Title = x.MetaTitle,
                                Keywords = x.MetaKeywords,
                                Description = x.MetaDescription,
                            }
                        })
                        .ToList()
                };

                _dbContext.Categories.Add(category);
                await _dbContext.SaveEntitiesAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
