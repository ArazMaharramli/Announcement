using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Settings.Commands.UpdateSetting
{
    public class UpdateSettingCommand : IRequest<bool>
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public class Handler : IRequestHandler<UpdateSettingCommand, bool>
        {
            private readonly IDbContext _dbContext;

            public Handler(IDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<bool> Handle(UpdateSettingCommand request, CancellationToken cancellationToken)
            {
                var setting = await _dbContext.Settings.FirstOrDefaultAsync(x => x.Key == request.Key);
                setting.Value = request.Value;
                _dbContext.Settings.Update(setting);
                return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
            }
        }
    }
}
