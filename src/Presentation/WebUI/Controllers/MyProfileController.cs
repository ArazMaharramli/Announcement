using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Rooms.Queries.GetRoomsOfCurrentUser;
using Application.CQRS.Users.Commands.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WebUI.Controllers;

[Authorize]
public class MyProfileController : BaseController
{
    public IActionResult PersonalInfo()
    {
        var model = new UpdateUserCommand
        {
            Id = CurrentUserService.UserId,
            Name = CurrentUserService.User.Name,
            Email = CurrentUserService.User.Email,
            PhoneNumber = CurrentUserService.User.Phone
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> PersonalInfoAsync(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(command, cancellationToken);
        return RedirectToAction("Index", "Rooms");
    }

    public async Task<IActionResult> RoomsAsync(CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(new GetRoomsOfCurrentUserQuery(), cancellationToken);

        return View(res);
    }
}

