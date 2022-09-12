using System;
using System.Linq;
using System.Threading.Tasks;
using Application.CQRS.Requirements.Commands.Create;
using Application.CQRS.Requirements.Commands.Delete;
using Application.CQRS.Requirements.Commands.AddOrUpdateTranslation;
using Application.CQRS.Requirements.Queries.FindById;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebUI.Controllers;
using WebUI.Models.ConfigModels;
using Application.CQRS.Requirements.Commands.Update;
using Application.CQRS.Requirements.Queries.Search;
using Application.CQRS.Categories.Queries.GetAll;
using MediatR;
using System.Threading;
using Application.CQRS.Requirements.Queries.GetAll;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RequirementsController : BaseController
    {
        private readonly IStringLocalizer<RequirementsController> _localizer;
        private readonly SupportedLanguages _supportedLanguages;

        public RequirementsController(IStringLocalizer<RequirementsController> localizer, SupportedLanguages supportedLanguages)
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

            var query = new SearchRequirementsQuery
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
            var model = new CreateRequirementCommand
            {
                Translations = _supportedLanguages.Languages.Select(x => new RequirementTranslationVM { LangCode = x.Culture, Name = "" }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateRequirementCommand command)
        {
            var resp = await Mediator.Send(command);
            return RedirectToAction(nameof(Index), "Requirements", new { Area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var requirement = await Mediator.Send(new FindByRequirementIdQuery
            {
                Id = id
            });
            var model = new UpdateRequirementCommand
            {
                Id = requirement.Id,
                Icon = requirement.Icon,
                Translations = requirement.Translations
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateRequirementCommand command)
        {
            var resp = await Mediator.Send(command);
            return RedirectToAction(nameof(Index), "Requirements", new { Area = "Admin" });
        }

        [HttpGet("{lang}/[area]/[controller]/[action]/{langCode}/{id}")]
        public async Task<IActionResult> EditTranslation([FromRoute] string langCode, [FromRoute] string id)
        {
            var requirement = await Mediator.Send(new FindByRequirementIdQuery
            {
                Id = id,
            });
            var model = new AddOrUpdateRequirementTranslationCommand
            {
                Id = id,
                LangCode = langCode,
                Name = requirement.Translations?.FirstOrDefault(x => x.LangCode == langCode)?.Name
            };
            return View(model);
        }

        [HttpPost("{lang}/[area]/[controller]/[action]/{langCode}/{id}")]
        public async Task<IActionResult> EditTranslation(AddOrUpdateRequirementTranslationCommand command)
        {

            var resp = await Mediator.Send(command);

            return RedirectToAction(nameof(Index), "Requirements", new { Area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] DeleteRequirementsCommand command)
        {
            var resp = await Mediator.Send(command);

            return resp ? Ok() : BadRequest(new { message = _localizer["ErrorMessage"].Value });
        }

        public async Task<IActionResult> All(CancellationToken cancellationToken)
        {
            var resp = await Mediator.Send(new GetAllRequirementsQuery(), cancellationToken);
            return Ok(resp);
        }
    }

}
