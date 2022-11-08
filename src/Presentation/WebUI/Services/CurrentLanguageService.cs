using System.Linq;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using WebUI.Models.ConfigModels;

namespace WebUI.Services
{
    public class CurrentLanguageService : ICurrentLanguageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentLanguageService(IHttpContextAccessor httpContextAccessor, SupportedLanguages supportedLanguages)
        {
            _httpContextAccessor = httpContextAccessor;
            var rqf = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();
            LangCode = rqf.RequestCulture.Culture.Name;

            var currentlang = supportedLanguages.Languages.FirstOrDefault(x => x.Culture == LangCode);
            DisplayName = currentlang.DisplayName;
            IsDefault = currentlang.IsDefault;
        }

        public string LangCode { get; }
        public string DisplayName { get; }
        public bool IsDefault { get; set; }
    }
}
