using System;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Amenities.Queries.GetAll;
using Application.CQRS.Categories.Queries.GetAll;
using Application.CQRS.Requirements.Queries.GetAll;
using Application.CQRS.Rooms.Commands.Create;
using Application.CQRS.Rooms.Queries.GetActiveRooms;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class RoomsController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateRoomCommand command, CancellationToken cancellationToken)
        {
            var res = await Mediator.Send(command, cancellationToken);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> All(string query, string categoryId, DateTime? startFrom, CancellationToken cancellationToken)
        {
            var resp = await Mediator.Send(new GetActiveRoomsQuery { CategoryId = categoryId, StartFrom = startFrom, Query = query }, cancellationToken);
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
}
