using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOS;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Categories.Queries.FindById
{
    public class FindByCategoryIdQuery : IRequest<CategoryDto>
    {
        public string Id { get; set; }

        public class Handler : IRequestHandler<FindByCategoryIdQuery, CategoryDto>
        {
            private readonly IDbContext _dbContext;
            private readonly IMapper _mapper;

            public Handler(IDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;
            }

            public async Task<CategoryDto> Handle(FindByCategoryIdQuery request, CancellationToken cancellationToken)
            {
                var category = await _dbContext.Categories
                    .Include(x => x.Translations)
                    .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                if (category is null)
                {
                    throw new NotFoundException(nameof(Category), request.Id);
                }

                return category;
            }
        }
    }
}
