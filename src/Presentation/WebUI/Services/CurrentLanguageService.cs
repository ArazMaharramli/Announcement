using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace WebUI.Services
{
    public class CurrentLanguageService : ICurrentLanguageService
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentLanguageService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            var rqf = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>();
            LangCode = rqf.RequestCulture.Culture.Name;
        }

        public string LangCode { get; }
    }
}
