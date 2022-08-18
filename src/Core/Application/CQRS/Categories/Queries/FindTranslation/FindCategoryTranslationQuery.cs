using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Categories.Queries.FindTranslation
{
    public class FindCategoryTranslationQuery : IRequest<CategoryDetailVM>
    {
        public string Id { get; set; }
        public string LangCode { get; set; }

        public class Handler : IRequestHandler<FindCategoryTranslationQuery, CategoryDetailVM>
        {
            private readonly IDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(IDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<CategoryDetailVM> Handle(FindCategoryTranslationQuery request, CancellationToken cancellationToken)
            {
                var categoryTranslation = await _dbContext.CategoryTranslations
                    .Include(x => x.Category)
                    .AsNoTracking()
                    .ProjectTo<CategoryDetailVM>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id || x.Lang == request.LangCode, cancellationToken);

                if (categoryTranslation is null)
                {
                    throw new NotFoundException(nameof(CategoryTranslation), request.Id);
                }

                return categoryTranslation;
            }
        }
    }
}

