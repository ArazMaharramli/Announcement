using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Users.Commands.Update;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
}

