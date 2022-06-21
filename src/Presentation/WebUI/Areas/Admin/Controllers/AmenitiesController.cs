using System;
using System.Linq;
using System.Threading.Tasks;
using Application.CQRS.Amenities.Commands.Create;
using Application.CQRS.Amenities.Commands.Delete;
using Application.CQRS.Amenities.Commands.AddOrUpdateAmenitieTranslation;
using Application.CQRS.Amenities.Queries.FindById;
using Application.CQRS.Amenities.Queries.SearchAmenities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebUI.Controllers;
using WebUI.Models.ConfigModels;
using Application.CQRS.Amenities.Commands.Update;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AmenitiesController : BaseController
    {
        private readonly IStringLocalizer<AmenitiesController> _localizer;
        private readonly SupportedLanguages _supportedLanguages;

        public AmenitiesController(IStringLocalizer<AmenitiesController> localizer, SupportedLanguages supportedLanguages)
        {
            _localizer = localizer;
            _supportedLanguages = supportedLanguages;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        public async Task<IActionResult> DatatableAsync()
        {
            var searchLang = "";//Request.Form["search[language]"].FirstOrDefault();
            var start = Request.Query["start"].FirstOrDefault();
            var length = Request.Query["length"].FirstOrDefault();
            var sortColumnIndex = Request.Query["order[0][column]"].FirstOrDefault();
            var sortColumn = Request.Query[$"columns[{sortColumnIndex}][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Query["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Query["search[value]"].FirstOrDefault();
            int pageSize = string.IsNullOrEmpty(length) ? 10 : Convert.ToInt32(length);
            int skip = string.IsNullOrEmpty(start) ? 0 : Convert.ToInt32(start);

            var lang = string.IsNullOrEmpty(searchLang) ? RouteData.Values["lang"].ToString() : searchLang;

            var query = new SearchAmenitiesQuery
            {
                Deleted = false,
                LangCode = lang,
                PageSize = pageSize,
                SearchValue = searchValue,
                Page = skip / pageSize + 1,
                SortColumn = sortColumn,
                SortColumnDirection = sortColumnDirection
            };
            var resp = await Mediator.Send(query);
            return Ok(resp);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateAmenitieCommand
            {
                Translations = _supportedLanguages.Languages.Select(x => new AmenitieNameTranslationVM { LangCode = x.Culture, Name = "" }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateAmenitieCommand command)
        {
            var resp = await Mediator.Send(command);
            return RedirectToAction(nameof(Index), "Amenities", new { Area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var amenitie = await Mediator.Send(new FindByAmenitieIdQuery
            {
                Id = id
            });
            var model = new UpdateAmenitieCommand
            {
                Id = amenitie.Id,
                Icon = amenitie.Icon,
                Translations = amenitie.Translations
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateAmenitieCommand command)
        {
            var resp = await Mediator.Send(command);
            return RedirectToAction(nameof(Index), "Amenities", new { Area = "Admin" });
        }

        [HttpGet("{lang}/[area]/[controller]/[action]/{langCode}/{id}")]
        public async Task<IActionResult> EditTranslation([FromRoute] string langCode, [FromRoute] string id)
        {
            var amenitie = await Mediator.Send(new FindByAmenitieIdQuery
            {
                Id = id,
            });
            var model = new AddOrUpdateAmenitieTranslationCommand
            {
                Id = id,
                LangCode = langCode,
                Name = amenitie.Translations?.FirstOrDefault(x => x.LangCode == langCode)?.Name
            };
            return View(model);
        }

        [HttpPost("{lang}/[area]/[controller]/[action]/{langCode}/{id}")]
        public async Task<IActionResult> EditTranslation(AddOrUpdateAmenitieTranslationCommand command)
        {

            var resp = await Mediator.Send(command);

            return RedirectToAction(nameof(Index), "Amenities", new { Area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteAmenitiesCommand command)
        {
            var resp = await Mediator.Send(command);

            return resp ? Ok() : BadRequest(new { message = _localizer["ErrorMessage"].Value });
        }
    }
}
