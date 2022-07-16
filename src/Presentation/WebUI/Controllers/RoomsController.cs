using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Amenities.Queries.GetAll;
using Application.CQRS.Categories.Queries.GetAll;
using Application.CQRS.Requirements.Queries.GetAll;
using Application.CQRS.Rooms.Commands.Create;
using Application.CQRS.RoomTypes.Queries.GetAll;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    public class RoomsController : BaseController
    {
        // GET: /<controller>/
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

        public async Task<IActionResult> RoomTypes(CancellationToken cancellationToken)
        {
            var resp = await Mediator.Send(new GetAllRoomTypesQuery(), cancellationToken);
            return Ok(resp);
        }
        public async Task<IActionResult> Categories(CancellationToken cancellationToken)
        {
            var resp = await Mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
            return Ok(resp);
        }
    }
}
