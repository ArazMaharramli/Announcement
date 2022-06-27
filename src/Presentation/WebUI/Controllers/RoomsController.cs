using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.CQRS.Rooms.Commands.Create;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
            var rqf = Request.HttpContext.Features.Get<IRequestCultureFeature>();
            // Culture contains the information of the requested culture
            var culture = rqf.RequestCulture.Culture;
            var lang = culture.Name;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateRoomCommand command, CancellationToken cancellationToken)
        {
            var res = await Mediator.Send(command, cancellationToken);
            return RedirectToAction("Index", "Home");
        }
    }
}
