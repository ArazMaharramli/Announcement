using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Localization.SqlLocalizer.DbStringLocalizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Areas.Admin.ViewModels.Translations;
using WebUI.Models.ConfigModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TranslationsController : Controller
    {
        private readonly IStringExtendedLocalizerFactory _stringExtendedLocalizerFactory;
        private readonly SupportedLanguages _supportedLanguages;
        private readonly LocalizationModelContext _dbContext;
        private readonly IMapper _mapper;

        public TranslationsController(LocalizationModelContext dbContext, IMapper mapper, SupportedLanguages supportedLanguages, IStringExtendedLocalizerFactory stringExtendedLocalizerFactory)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _supportedLanguages = supportedLanguages;
            _stringExtendedLocalizerFactory = stringExtendedLocalizerFactory;
        }

        public async Task<IActionResult> IndexAsync(CancellationToken cancellationToken)
        {
            var resourceKeys = await _dbContext.LocalizationRecords
                .Select(x => x.ResourceKey)
                .Distinct()
                .ToListAsync(cancellationToken);

            var model = new TranslationsIndexViewModel
            {
                ResourceKeys = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(resourceKeys)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DatatableAsync(CancellationToken cancellationToken)
        {
            var searchLang = Request.Form["query[language]"].FirstOrDefault();
            var currentPage = Request.Form["pagination[page]"].FirstOrDefault();
            var length = Request.Form["pagination[perpage]"].FirstOrDefault();
            var sortColumn = Request.Form["sort[field]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["sort[sort]"].FirstOrDefault();
            var searchValue = Request.Form["query[generalSearch]"].FirstOrDefault();
            var resource = Request.Form["query[resource]"].FirstOrDefault() ?? "";

            int pageSize = string.IsNullOrEmpty(length) ? 10 : Convert.ToInt32(length);
            int page = string.IsNullOrEmpty(currentPage) ? 1 : Convert.ToInt32(currentPage);
            var lang = string.IsNullOrEmpty(searchLang) ? RouteData.Values["lang"].ToString() : searchLang;

            sortColumn = string.IsNullOrEmpty(sortColumn) ? nameof(TranslationViewModel.UpdatedAt) : sortColumn;
            sortColumnDirection = string.IsNullOrEmpty(sortColumnDirection) ? "asc" : sortColumnDirection;


            var list = await _dbContext.LocalizationRecords
                .Where(y => y.ResourceKey.ToLower() == resource && (y.Key.Contains(searchValue) ||
                        y.Text.Contains(searchValue) || true))
                .ToListAsync(cancellationToken);

            var datas = new List<TranslationViewModel>();
            var newList = list;

            foreach (var item in list.Where(x => !datas.Any(y => y.Key == x.Key)))
            {
                var key = item.Key;
                var translations = list.Where(x => x.Key == key).Select(x => new TranslationDto
                {
                    Id = x.Id,
                    LangCode = x.LocalizationCulture,
                    Text = x.Text
                }).ToList();
                datas.Add(new TranslationViewModel
                {
                    Key = key,
                    Text = translations.FirstOrDefault(x => x.LangCode.ToLower() == lang)?.Text ?? "---",
                    UpdatedAt = translations.Min(x => x.UpdatedAt),
                    Translations = translations
                });
            }


            var resp = new DataTablePagedList<TranslationViewModel>(
                data: datas,
                total: datas.Count,
                page: page,
                perpage: datas.Count,
                sortColumn: sortColumn,
                sortDir: sortColumnDirection
                );
            return Ok(resp);
        }

        [HttpGet("{lang}/[area]/[controller]/[action]/{langCode}/{resource}/{key}")]
        public async Task<IActionResult> EditTranslation([FromRoute] string key, [FromRoute] string resource, [FromRoute] string langCode, CancellationToken cancellationToken)
        {
            var translation = await _dbContext.LocalizationRecords.FirstOrDefaultAsync(
                x =>
                x.ResourceKey.ToLower() == resource.ToLower() &&
                x.Key.ToLower() == key.ToLower() &&
                x.LocalizationCulture.ToLower() == langCode.ToLower(), cancellationToken);
            var model = new EditTranslationViewModel
            {
                Id = translation.Id,
                Key = translation.Key,
                Text = translation.Text,
                LangCode = translation.LocalizationCulture,
                ResourceKey = translation.ResourceKey
            };
            return View(model);
        }

        [HttpPost("{lang}/[area]/[controller]/[action]/{langCode}/{resource}/{key}")]
        public async Task<IActionResult> EditTranslation(EditTranslationViewModel model, CancellationToken cancellationToken)
        {
            var translation = await _dbContext.LocalizationRecords.FirstOrDefaultAsync(
                x =>
                x.ResourceKey.ToLower() == model.ResourceKey.ToLower() &&
                x.Key.ToLower() == model.Key.ToLower() &&
                x.LocalizationCulture.ToLower() == model.LangCode.ToLower(), cancellationToken);

            if (translation is null)
            {
                translation = new LocalizationRecord
                {
                    Key = model.Key,
                    ResourceKey = model.ResourceKey,
                    LocalizationCulture = model.LangCode,
                    Text = model.Text
                };
                _dbContext.LocalizationRecords.Add(translation);
            }
            else
            {
                translation.Text = model.Text;
                _dbContext.LocalizationRecords.Update(translation);
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
            _stringExtendedLocalizerFactory.ResetCache();
            return RedirectToAction("Index", "Translations", new { Areaa = "Admin" });
        }
    }
}
