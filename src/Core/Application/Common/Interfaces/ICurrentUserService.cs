
using Application.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    string UserId { get; }

    bool IsAuthenticated { get; }

    public UserDTO User { get; }

}
