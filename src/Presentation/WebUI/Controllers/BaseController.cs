using System;
using System.Linq;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using WebUI.Extentions.HttpRequestExtentions;
using WebUI.Models.ConfigModels;

namespace WebUI.Controllers;

public abstract class BaseController : Controller
{
    private IMediator _mediator;

    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    protected ICurrentUserService CurrentUserService => _currentUserService ?? HttpContext.RequestServices.GetService<ICurrentUserService>();

    private ICurrentLanguageService _currentLanguage => HttpContext.RequestServices.GetService<ICurrentLanguageService>();
    private ICurrentUserService _currentUserService => HttpContext.RequestServices.GetService<ICurrentUserService>();
    protected SupportedLanguages _supportedLanguages => HttpContext.RequestServices.GetService<SupportedLanguages>();

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (Request.IsAjaxRequest())
        {
            return;
        }

        var a = (Request.RouteValues["lang"])?.ToString();
        if (a is null && _currentLanguage.IsDefault)
        {
            return;
        }

        var selectedLang = _supportedLanguages.Languages.FirstOrDefault(x => x.Culture == a);

        if (a is not null && selectedLang is not null && _currentLanguage.LangCode != a)
        {
            context.HttpContext.Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(selectedLang.Culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return;
        }
        //&& !_currentLanguage.IsDefault
        if (a is not null && a != _currentLanguage.LangCode)
        {
            context.HttpContext.Request.RouteValues["lang"] = _currentLanguage.LangCode;
            context.Result = RedirectToRoute(context.HttpContext.Request.RouteValues);
        }

        return;
    }
}
