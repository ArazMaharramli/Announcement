using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Categories.Commands.Delete
{
    public class DeleteCategoriesCommand : IRequest<bool>
    {
        public string[] Ids { get; set; }
        public class Handler : IRequestHandler<DeleteCategoriesCommand, bool>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(DeleteCategoriesCommand request, CancellationToken cancellationToken)
            {
                var categories = await _dbContext.Categories.Where(x => request.Ids.Contains(x.Id)).ToListAsync(cancellationToken);
                _dbContext.Categories.RemoveRange(categories);

                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}
