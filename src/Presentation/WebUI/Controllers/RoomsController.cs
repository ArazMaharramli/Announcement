using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Amenities.Queries.GetAll;
using Application.CQRS.Categories.Queries.GetAll;
using Application.CQRS.Requirements.Queries.GetAll;
using Application.CQRS.Rooms.Commands.Create;
using Application.CQRS.Rooms.Queries.FindBySlug;
using Application.CQRS.Rooms.Queries.GetActiveRooms;
using Microsoft.AspNetCore.Mvc;
using WebUI.Models.ViewModels.Rooms;

namespace WebUI.Controllers;

public class RoomsController : BaseController
{

    public async Task<IActionResult> IndexAsync(CancellationToken cancellationToken)
    {
        var resp = await Mediator.Send(new GetActiveRoomsQuery(), cancellationToken);
        var model = new RoomsIndexViewModel
        {
            Rooms = resp.Data
        };
        return View(model);
    }

    public IActionResult Search(CancellationToken cancellationToken)
    {
        return View();
    }

    public IActionResult Create()
    {
        if (CurrentUserService.IsAuthenticated)
        {
            var model = new CreateRoomCommand
            {
                ContactEmail = CurrentUserService.User.Email,
                ContactName = CurrentUserService.User.Name,
                ContactPhone = CurrentUserService.User.Phone
            };
            return View(model);
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateRoomCommand command, CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(command, cancellationToken);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Route("[controller]/[action]/{slug}")]
    public async Task<IActionResult> Details(string slug, CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(new FindRoomBySlugQuery { Slug = slug }, cancellationToken);
        return View(res);
    }

    [HttpPost]
    public async Task<IActionResult> All(RoomsScrollList model, CancellationToken cancellationToken)
    {
        var resp = await Mediator.Send(new GetActiveRoomsQuery
        {
            CategoryId = model.CategoryId,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            Query = model.Query,
            Lat = model.Lat,
            Lng = model.Lng,
        }, cancellationToken);

        return Ok(resp);
    }

    public async Task<IActionResult> Amenities(CancellationToken cancellationToken)
    {
        var resp = await Mediator.Send(new GetAllAmenitiesQuery(), cancellationToken);
        return Ok(resp);
    }

    public async Task<IActionResult> Requirements(CancellationToken cancellationToken)
    {
        var resp = await Mediator.Send(new GetAllRequirementsQuery(), cancellationToken);
        return Ok(resp);
    }

    public async Task<IActionResult> Categories(CancellationToken cancellationToken)
    {
        var resp = await Mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
        return Ok(resp);
    }
}
