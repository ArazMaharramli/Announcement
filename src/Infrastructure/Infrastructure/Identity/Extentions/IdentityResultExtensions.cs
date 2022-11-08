using System;
using System.Linq;
using Application.Common.Models;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Extentions
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Description));
        }
    }


    public static class ToApplicationUserDTOExtentions
    {
        public static UserDTO ToApplicationUserDTO(this ApplicationUser user)
        {
            return new UserDTO(user.Id, user.Name, user.UserName, user.Email, user.PhoneNumber, user.ProfilePictureUrl);
        }
    }
}
