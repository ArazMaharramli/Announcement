using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Common;
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

    public class CategoryDetailVM : IMapFrom<CategoryTranslation>
    {
        public string Id { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Lang { get; set; }

        public Meta Meta { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CategoryTranslation, CategoryDetailVM>()
                .ForMember(x => x.Meta, opt => opt.UseDestinationValue())
                .ForMember(x => x.Id, opt => opt.MapFrom(y => y.Category.Id))
                .ForMember(x => x.Lang, opt => opt.MapFrom(y => y.LangCode))
                .ForMember(x => x.Icon, opt => opt.MapFrom(y => y.Category.Icon));
        }
    }
}

