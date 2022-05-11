using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Settings.Queries.GetAllSettings
{
    public class GetAllSettingsQuery : IRequest<List<Setting>>
    {
        public class Handler : IRequestHandler<GetAllSettingsQuery, List<Setting>>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public Task<List<Setting>> Handle(GetAllSettingsQuery request, CancellationToken cancellationToken)
            {
                return _dbContext.Settings.ToListAsync(cancellationToken);
            }
        }
    }
}
