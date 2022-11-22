using System.Collections.Generic;
using Application.Common.Models;
using Infrastructure.Identity.Entities;

namespace Infrastructure.Identity.Extentions
{
    public static class ToApplicationEntityDTOExtentions
    {
        public static UserDTO ToApplicationUserDTO(this ApplicationUser user)
        {
            return new UserDTO(user.Id, user.Name, user.UserName, user.Email, user.PhoneNumber, user.ProfilePictureUrl);
        }

        public static RoleDto ToApplicationRoleDTO(this ApplicationRole role, int userCount)
        {
            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                UsersCount = userCount,
                Claims = new List<string>()
            };
        }
    }


}
