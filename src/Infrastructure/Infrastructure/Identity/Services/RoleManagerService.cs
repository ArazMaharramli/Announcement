using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Extentions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Services
{
    public class RoleManagerService : IRoleManager
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;

        public RoleManagerService(RoleManager<ApplicationRole> roleManager, ApplicationDbContext dbContext)
        {
            _roleManager = roleManager;
            _dbContext = dbContext;
        }

        public async Task<List<RoleDto>> GetAll(CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles
                .ToListAsync(cancellationToken);

            return roles.Select(x =>
                    x.ToApplicationRoleDTO(_dbContext.RoleClaims.Count(z => z.RoleId == x.Id))
                )
                .ToList();
        }

        public async Task<(Result Result, string RoleId)> CreateAsync(string id, string roleName, List<string> claims)
        {
            var role = new ApplicationRole { Id = id, Name = roleName };

            var roleRes = await _roleManager.CreateAsync(role);
            if (roleRes.Succeeded)
            {
                foreach (var claimName in claims)
                {
                    var claimRes = await _roleManager.AddClaimAsync(role, new Claim(ClaimTypes.UserData, claimName));
                }
            }
            return (roleRes.ToApplicationResult(), role.Id);
        }


        public async Task<List<string>> GetClaims(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role is null) return null;

            var claims = await _roleManager.GetClaimsAsync(role);

            var claimList = claims.Select(x => x.Value).ToList();

            return claimList;
        }


        public async Task<RoleDto> FindByName(string roleName)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(x => x.Name == roleName);

            if (role is null) return null;

            var userCount = await _dbContext.UserRoles.CountAsync(x => x.RoleId == role.Id);

            return role.ToApplicationRoleDTO(userCount);
        }

        public async Task<RoleDto> FindById(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null) return null;

            var userCount = await _dbContext.UserRoles.CountAsync(x => x.RoleId == role.Id);

            return role.ToApplicationRoleDTO(userCount);
        }

        public async Task<int> GetUserCountByRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role is null) return 0;

            var userCount = await _dbContext.UserRoles.CountAsync(x => x.RoleId == role.Id);

            return userCount;
        }


        public async Task<bool> UpdateByIdAsync(string roleId, string roleName)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null) return false;

            if (role.Name != roleName)
            {
                role.Name = roleName;

                var result = await _roleManager.UpdateAsync(role);

                return result.Succeeded;
            }

            return true;
        }

        // Claims
        public async Task<List<string>> GetClaimsByRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role is null) return null;

            var claims = await _roleManager.GetClaimsAsync(role);

            if (claims.Count == 0) return null;

            List<string> claimValues = claims.Select(x => x.Value).ToList();

            return claimValues;
        }


        public async Task<Result> AddOneRoleClaim(string roleId, string claimValue)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            var c = new Claim(ClaimTypes.UserData, claimValue);

            var res = await _roleManager.AddClaimAsync(role, c);

            return res.ToApplicationResult();
        }


        public async Task<Result> RemoveOneRoleClaim(string roleId, string claimValue)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            var c = new Claim(ClaimTypes.UserData, claimValue);

            var res = await _roleManager.RemoveClaimAsync(role, c);

            return res.ToApplicationResult();
        }

        public async Task DeleteRole(string roleId, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                throw new NotFoundException(nameof(ApplicationRole), roleId);
            }
            await _roleManager.DeleteAsync(role);
        }
    }
}