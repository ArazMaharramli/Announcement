using System;
using System.Linq;
using System.Threading.Tasks;
using Application.CQRS.Requirements.Commands.CreateRequirement;
using Application.CQRS.Requirements.Commands.UpdateRequirementIcon;
using Application.CQRS.Requirements.Commands.DeleteRequirement;
using Application.CQRS.Requirements.Commands.AddOrUpdateRequirementTranslation;
using Application.CQRS.Requirements.Queries.FindByRequirementId;
using Application.CQRS.Requirements.Queries.SearchRequirements;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebUI.Areas.Admin.ViewModels.Requirements;
using WebUI.Controllers;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RequirementsController : BaseController
    {
        private readonly IStringLocalizer<RequirementsController> _localizer;
        public RequirementsController(IStringLocalizer<RequirementsController> localizer)
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

            var query = new SearchRequirementsQuery
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
        public async Task<IActionResult> CreateAsync(CreateRequirementViewModel model)
        {
            var command = new CreateRequirementCommand
            {
                Icon = model.Icon,
                Name = model.Name,
                LangCode = RouteData.Values["lang"].ToString()
            };
            var resp = await Mediator.Send(command);
            return RedirectToAction(nameof(Index), "Requirements", new { Area = "Admin" });
        }

        [HttpGet]
        public async Task<IActionResult> EditIcon(string id)
        {
            var requirement = await Mediator.Send(new FindByRequirementIdQuery
            {
                Id = id,
                LangCode = RouteData.Values["lang"].ToString()
            });
            var model = new EditRequirementIconViewModel
            {
                Id = requirement.Id,
                Icon = requirement.Icon,
                Name = requirement.Name,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditIconAsync(EditRequirementIconViewModel model)
        {
            var command = new UpdateRequirementIconCommand
            {
                Id = model.Id,
                Icon = model.Icon,
            };
            var resp = await Mediator.Send(command);
            return RedirectToAction(nameof(Index), "Requirements", new { Area = "Admin" });
        }

        [HttpGet("{lang}/[area]/[controller]/[action]/{langCode}/{id}")]
        public async Task<IActionResult> EditTranslation([FromRoute] string langCode, [FromRoute] string id)
        {
            var requirement = await Mediator.Send(new FindByRequirementIdQuery
            {
                Id = id,
                LangCode = langCode
            });
            var model = new EditRequirementTranslationViewModel
            {
                Id = id,
                LangCode = langCode,
                Name = requirement.Name
            };
            return View(model);
        }

        [HttpPost("{lang}/[area]/[controller]/[action]/{langCode}/{id}")]
        public async Task<IActionResult> EditTranslation(EditRequirementTranslationViewModel model)
        {
            var command = new AddOrUpdateRequirementTranslationCommand
            {
                Id = model.Id,
                Name = model.Name,
                LangCode = model.LangCode
            };
            var resp = await Mediator.Send(command);

            return RedirectToAction(nameof(Index), "Requirements", new { Area = "Admin" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteRequirementCommand
            {
                Id = id,
            };
            var resp = await Mediator.Send(command);

            return resp ? Ok() : BadRequest(new { message = _localizer["ErrorMessage"].Value });
        }
    }
}
