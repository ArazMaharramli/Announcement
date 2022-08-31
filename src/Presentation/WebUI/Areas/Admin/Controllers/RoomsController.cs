using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.CQRS.Rooms.Commands.Delete;
using Application.CQRS.Rooms.Queries.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebUI.Controllers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomsController : BaseController
    {
        private readonly IStringLocalizer<AmenitiesController> _localizer;

        public RoomsController(IStringLocalizer<AmenitiesController> localizer)
        {
            _localizer = localizer;
        }

        [HttpPost]
        public async Task<IActionResult> DatatableAsync(bool deleted = false)
        {
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = string.IsNullOrEmpty(length) ? 10 : Convert.ToInt32(length);
            int skip = string.IsNullOrEmpty(start) ? 0 : Convert.ToInt32(start);

            var query = new SearchRoomsQuery
            {
                Deleted = deleted,
                PageSize = pageSize,
                SearchValue = searchValue,
                Page = skip / pageSize + 1,
                SortColumn = sortColumn,
                SortColumnDirection = sortColumnDirection
            };
            var resp = await Mediator.Send(query);
            return Ok(resp);
        }

        // GET: /<controller>/
        public IActionResult Index(bool deleted = false)
        {
            ViewBag.Deleted = deleted;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteRoomsCommand command)
        {
            var resp = await Mediator.Send(command);

            return Ok();
        }
    }
}

