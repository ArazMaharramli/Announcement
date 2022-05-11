using System;
using System.Linq;
using System.Threading.Tasks;
using Application.CQRS.RoomTypes.Commands.AddOrUpdateRoomTypeTranslation;
using Application.CQRS.RoomTypes.Commands.CreateRoomType;
using Application.CQRS.RoomTypes.Commands.DeleteRoomType;
using Application.CQRS.RoomTypes.Commands.UpdateRoomTypeImage;
using Application.CQRS.RoomTypes.Queries.FindByRoomTypeId;
using Application.CQRS.RoomTypes.Queries.SearchRoomTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebUI.Areas.Admin.ViewModels.RoomTypes;
using WebUI.Controllers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoomTypesController : BaseController
    {
        private readonly IStringLocalizer<RoomTypesController> _localizer;
        public RoomTypesController(IStringLocalizer<RoomTypesController> localizer)
        {
            _localizer = localizer;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DatatableAsync()
        {
            var searchLang = Request.Form["query[language]"].FirstOrDefault();
            var currentPage = Request.Form["pagination[page]"].FirstOrDefault();
            var length = Request.Form["pagination[perpage]"].FirstOrDefault();
            var sortColumn = Request.Form["sort[field]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["sort[sort]"].FirstOrDefault();
            var searchValue = Request.Form["query[generalSearch]"].FirstOrDefault();
            int pageSize = string.IsNullOrEmpty(length) ? 10 : Convert.ToInt32(length);
            int page = string.IsNullOrEmpty(currentPage) ? 1 : Convert.ToInt32(currentPage);
            var lang = string.IsNullOrEmpty(searchLang) ? RouteData.Values["lang"].ToString() : searchLang;

            var query = new SearchRoomTypesQuery
            {
                Deleted = false,
                LangCode = lang,
                PageSize = pageSize,
                SearchValue = searchValue,
                Page = page,
                SortColumn = sortColumn,
                SortColumnDirection = sortColumnDirection
            };
            var resp = await Mediator.Send(query);
            return Ok(resp);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateRoomTypeViewModel model)
        {
            var command = new CreateRoomTypeCommand
            {
                Image = model.Image,
                Name = model.Name,
                LangCode = RouteData.Values["lang"].ToString()
            };
            var resp = await Mediator.Send(command);
            return RedirectToAction(nameof(Index), "RoomTypes", new { Area = "Admin" });
        }

        [HttpGet("{lang}/[area]/[controller]/[action]/{langCode}/{id}")]
        public async Task<IActionResult> EditTranslation([FromRoute] string langCode, [FromRoute] string id)
        {
            var roomType = await Mediator.Send(new FindByRoomTypeIdQuery
            {
                Id = id,
                LangCode = langCode
            });
            var model = new EditRoomTypeTranslationViewModel
            {
                Id = id,
                LangCode = langCode,
                Name = roomType.Name,

            };
            return View(model);
        }

        [HttpPost("{lang}/[area]/[controller]/[action]/{langCode}/{id}")]
        public async Task<IActionResult> EditTranslation(EditRoomTypeTranslationViewModel model)
        {
            var command = new AddOrUpdateRoomTypeTranslationCommand
            {
                Id = model.Id,
                Name = model.Name,
                LangCode = model.LangCode
            };
            var resp = await Mediator.Send(command);

            return RedirectToAction(nameof(Index), "RoomTypes", new { Area = "Admin" });
        }

        public async Task<IActionResult> UpdateImage([FromRoute] string id)
        {
            var roomType = await Mediator.Send(new FindByRoomTypeIdQuery
            {
                Id = id,
                LangCode = RouteData.Values["lang"].ToString()
            });
            var model = new EditRoomTypeImageViewModel
            {
                Id = id,
                Image = roomType.Image,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateImage(EditRoomTypeImageViewModel model)
        {
            var command = new UpdateRoomTypeImageCommand
            {
                Id = model.Id,
                Image = model.Image
            };
            var resp = await Mediator.Send(command);

            return RedirectToAction(nameof(Index), "RoomTypes", new { Area = "Admin" });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteRoomTypeCommand
            {
                Id = id,
            };
            var resp = await Mediator.Send(command);

            return resp ? Ok() : BadRequest(new { message = _localizer["ErrorMessage"].Value });
        }

    }
}
