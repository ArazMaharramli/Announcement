using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CQRS.Rooms.Queries.Search;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Rooms.Queries.GetRoomsOfCurrentUser;

public class GetRoomsOfCurrentUserQuery : IRequest<List<RoomVm>>
{
    public class Handler : IRequestHandler<GetRoomsOfCurrentUserQuery, List<RoomVm>>
    {
        private readonly IDbContext _dbContext;
        private readonly ICurrentLanguageService _currentLanguageService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public Handler(IDbContext dbContext, ICurrentLanguageService currentLanguageService, ICurrentUserService currentUserService, IMapper mapper)
        {
            _dbContext = dbContext;
            _currentLanguageService = currentLanguageService;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public Task<List<RoomVm>> Handle(GetRoomsOfCurrentUserQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.Rooms
                 .Where(x => x.OwnerId == _currentUserService.UserId)
                 .AsNoTracking()
                 .ProjectTo<RoomVm>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);
        }
    }
}

