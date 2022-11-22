using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface IRoleManager
    {
        Task<RoleDto> FindByName(string roleName);
        Task<RoleDto> FindById(string roleId);
        Task<List<RoleDto>> GetAll(CancellationToken cancellationToken);
        Task<(Result Result, string RoleId)> CreateAsync(string id, string roleName, List<string> claims);

        Task<List<string>> GetClaims(string roleName);

        Task<int> GetUserCountByRole(string roleName);

        Task<bool> UpdateByIdAsync(string roleId, string roleName);

        Task<List<string>> GetClaimsByRole(string roleName);
        Task<Result> AddOneRoleClaim(string roleId, string claimValue);
        Task<Result> RemoveOneRoleClaim(string roleId, string claimValue);
    }
}

