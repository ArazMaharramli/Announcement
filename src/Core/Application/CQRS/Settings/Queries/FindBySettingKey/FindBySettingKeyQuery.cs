using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Settings.Queries.FindBySettingKey
{
    public class FindBySettingKeyQuery : IRequest<Setting>
    {
        public string Key { get; set; }

        public class Handler : IRequestHandler<FindBySettingKeyQuery, Setting>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<Setting> Handle(FindBySettingKeyQuery request, CancellationToken cancellationToken)
            {
                var setting = await _dbContext.Settings.FirstOrDefaultAsync(x => x.Key == request.Key);
                if (setting is null)
                {
                    throw new NotFoundException(nameof(Setting), request.Key);
                }
                return setting;
            }
        }
    }
}
