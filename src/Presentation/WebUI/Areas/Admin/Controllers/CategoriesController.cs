using System;
using System.Linq;
using System.Threading.Tasks;
using Application.CQRS.Categories.Commands.Create;
using Application.CQRS.Categories.Commands.Delete;
using Application.CQRS.Categories.Commands.AddOrUpdateTranslation;
using Application.CQRS.Categories.Queries.FindById;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using WebUI.Controllers;
using WebUI.Models.ConfigModels;
using Application.CQRS.Categories.Commands.Update;
using Application.CQRS.Categories.Queries.Search;
using Application.CQRS.Categories.Queries.FindTranslation;
using Application.CQRS.Categories.Queries.GetAll;
using System.Threading;
using Application.Common.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoriesController : BaseController
{
    private readonly IStringLocalizer<CategoriesController> _localizer;
    private readonly SupportedLanguages _supportedLanguages;

    public CategoriesController(IStringLocalizer<CategoriesController> localizer, SupportedLanguages supportedLanguages)
    {
        _localizer = localizer;
        _supportedLanguages = supportedLanguages;
    }
    // GET: /<controller>/
    [Authorize(Policy = SystemClaims.Categories.Show)]
    public IActionResult Index()
    {
        return View();
    }

    //[HttpPost]
    [Authorize(Policy = SystemClaims.Categories.Show)]
    public async Task<IActionResult> DatatableAsync(CancellationToken cancellationToken)
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

        var query = new SearchCategoriesQuery
        {
            Deleted = false,
            LangCode = lang,
            PageSize = pageSize,
            SearchValue = searchValue,
            Page = skip / pageSize + 1,
            SortColumn = sortColumn,
            SortColumnDirection = sortColumnDirection
        };
        var resp = await Mediator.Send(query, cancellationToken);
        return Ok(resp);
    }

    [HttpGet]
    [Authorize(Policy = SystemClaims.Categories.Create)]
    public IActionResult Create()
    {
        var model = new CreateCategoryCommand
        {
            Translations = _supportedLanguages.Languages
            .Select(x =>
            new CategoryTranslationVM
            {
                LangCode = x.Culture,
                Name = ""
            })
            .ToList()
        };
        return View(model);
    }

    [HttpPost]
    [Authorize(Policy = SystemClaims.Categories.Create)]
    public async Task<IActionResult> CreateAsync(CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var resp = await Mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index), "Categories", new { Area = "Admin" });
    }

    [HttpGet]
    [Authorize(Policy = SystemClaims.Categories.Edit)]
    public async Task<IActionResult> Edit(string id, CancellationToken cancellationToken)
    {
        var category = await Mediator.Send(new FindByCategoryIdQuery
        {
            Id = id
        }, cancellationToken);
        var model = new UpdateCategoryCommand
        {
            Id = category.Id,
            Icon = category.Icon,
            Translations = category.Translations.Select(x => new CategoryTranslationVM
            {
                LangCode = x.LangCode,
                Name = x.Name,
                MetaTitle = x.Meta.Title,
                MetaKeywords = x.Meta.Keywords,
                MetaDescription = x.Meta.Description
            }).ToList()
        };
        return View(model);
    }

    [HttpPost]
    [Authorize(Policy = SystemClaims.Categories.Edit)]
    public async Task<IActionResult> Edit(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        var resp = await Mediator.Send(command, cancellationToken);
        return RedirectToAction(nameof(Index), "Categories", new { Area = "Admin" });
    }

    [HttpGet("{lang}/[area]/[controller]/[action]/{langCode}/{id}")]
    [Authorize(Policy = SystemClaims.Categories.Edit)]
    public async Task<IActionResult> EditTranslation([FromRoute] string lang, [FromRoute] string langCode, [FromRoute] string id, CancellationToken cancellationToken)
    {
        var category = await Mediator.Send(new FindCategoryTranslationQuery
        {
            Id = id,
            LangCode = lang
        }, cancellationToken);
        var model = new AddOrUpdateCategoryTranslationCommand
        {
            Id = id,
            LangCode = langCode,
            Name = category.Name,
            MetaTitle = category.Meta.Title,
            MetaKeywords = category.Meta.Keywords,
            MetaDescription = category.Meta.Description
        };
        return View(model);
    }

    [HttpPost("{lang}/[area]/[controller]/[action]/{langCode}/{id}")]
    [Authorize(Policy = SystemClaims.Categories.Edit)]
    public async Task<IActionResult> EditTranslation(AddOrUpdateCategoryTranslationCommand command, CancellationToken cancellationToken)
    {

        var resp = await Mediator.Send(command, cancellationToken);

        return RedirectToAction(nameof(Index), "Categories", new { Area = "Admin" });
    }

    [HttpPost]
    [Authorize(Policy = SystemClaims.Categories.Delete)]
    public async Task<IActionResult> Delete([FromBody] DeleteCategoriesCommand command, CancellationToken cancellationToken)
    {
        var resp = await Mediator.Send(command, cancellationToken);

        return resp ? Ok() : BadRequest(new { message = _localizer["ErrorMessage"].Value });
    }

    public async Task<IActionResult> All(CancellationToken cancellationToken)
    {
        var resp = await Mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
        return Ok(resp);
    }
}
