using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.CQRS.Amenities.Commands.CreateAmenitie;
using Application.CQRS.Amenities.Commands.UpdateAmenitieIcon;
using Application.CQRS.Amenities.Commands.DeleteAmenitie;
using Application.CQRS.Amenities.Commands.AddOrUpdateAmenitieTranslation;
using Application.CQRS.Amenities.Queries.FindByAmenitieId;
using Application.CQRS.Amenities.Queries.SearchAmenities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebUI.Areas.Admin.ViewModels.Amenities;
using WebUI.Controllers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AmenitiesController : BaseController
    {
        private readonly IStringLocalizer<AmenitiesController> _localizer;
        public AmenitiesController(IStringLocalizer<AmenitiesController> localizer)
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

            var query = new SearchAmenitiesQuery
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
        public async Task<IActionResult> CreateAsync(CreateAmenitieViewModel model)
        {
            var command = new CreateAmenitieCommand
            {
                Icon = model.Icon,
                Name = model.Name,
                LangCode = RouteData.Values["lang"].ToString()
            };
            var resp = await Mediator.Send(command);
            return RedirectToAction(nameof(Index), "Amenities", new { Area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> EditIcon(string id)
        {
            var amenitie = await Mediator.Send(new FindByAmenitieIdQuery
            {
                Id = id,
                LangCode = RouteData.Values["lang"].ToString()
            });
            var model = new EditAmenitieIconViewModel
            {
                Id = amenitie.Id,
                Icon = amenitie.Icon,
                Name = amenitie.Name,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditIconAsync(EditAmenitieIconViewModel model)
        {
            var command = new UpdateAmenitieIconCommand
            {
                Id = model.Id,
                Icon = model.Icon,
            };
            var resp = await Mediator.Send(command);
            return RedirectToAction(nameof(Index), "Amenities", new { Area = "Admin" });
        }

        [HttpGet("{lang}/[area]/[controller]/[action]/{langCode}/{id}")]
        public async Task<IActionResult> EditTranslation([FromRoute] string langCode, [FromRoute] string id)
        {
            var amenitie = await Mediator.Send(new FindByAmenitieIdQuery
            {
                Id = id,
                LangCode = langCode
            });
            var model = new EditAmenitieTranslationViewModel
            {
                Id = id,
                LangCode = langCode,
                Name = amenitie.Name
            };
            return View(model);
        }

        [HttpPost("{lang}/[area]/[controller]/[action]/{langCode}/{id}")]
        public async Task<IActionResult> EditTranslation(EditAmenitieTranslationViewModel model)
        {
            var command = new AddOrUpdateAmenitieTranslationCommand
            {
                Id = model.Id,
                Name = model.Name,
                LangCode = model.LangCode
            };
            var resp = await Mediator.Send(command);

            return RedirectToAction(nameof(Index), "Amenities", new { Area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteAmenitieCommand
            {
                Id = id,
            };
            var resp = await Mediator.Send(command);

            return resp ? Ok() : BadRequest(new { message = _localizer["ErrorMessage"].Value });
        }
    }
}
