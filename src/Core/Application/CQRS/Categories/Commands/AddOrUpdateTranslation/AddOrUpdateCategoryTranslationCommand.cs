using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Categories.Commands.AddOrUpdateTranslation
{
    public class AddOrUpdateCategoryTranslationCommand : IRequest<bool>
    {
        public string Id { get; set; }
        public string LangCode { get; set; }
        public string Name { get; set; }

        public string MetaTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }

        public class Handler : IRequestHandler<AddOrUpdateCategoryTranslationCommand, bool>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(AddOrUpdateCategoryTranslationCommand request, CancellationToken cancellationToken)
            {
                var category = await _dbContext.Categories
                    .Include(x => x.Translations)
                    .FirstOrDefaultAsync(
                        x => !x.Deleted && x.Id == request.Id,
                    cancellationToken);

                if (category is null)
                {
                    throw new NotFoundException(nameof(Category), request.Id);
                }

                request.Name = request.Name.Trim();
                var translation = category.Translations.FirstOrDefault(x => x.LangCode == request.LangCode);
                if (translation is not null)
                {
                    translation.Name = request.Name;
                    translation.Meta.Title = request.MetaTitle;
                    translation.Meta.Keywords = request.MetaKeywords;
                    translation.Meta.Description = request.MetaDescription;

                }
                else
                {
                    category.Translations.Add(new CategoryTranslation
                    {
                        LangCode = request.LangCode,
                        Name = request.Name
                    });
                }


                _dbContext.Categories.Update(category);
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}
