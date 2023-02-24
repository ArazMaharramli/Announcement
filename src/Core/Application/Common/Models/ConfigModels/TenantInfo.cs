using System.Collections.Generic;

namespace Application.Common.Models.ConfigModels;

/// <summary>
/// This will be object containing tenant info with translations to currently requested language
/// All info of current and all other tenants will be stored in another object
/// </summary>
public class TenantInfo
{
    public string Domain { get; set; }
    public string Name { get; set; }
    public string Logo { get; set; }
    public string Icon { get; set; }
    public string FaviconIco { get; set; }
    public string ShortDescription { get; set; }
    public string Slogan { get; set; }

    public StaticPages StaticPages { get; set; }
    public ProjectContact Contact { get; set; }
    public SmtpOptions SmtpOptions { get; set; }
    public EmailTemplates EmailTemplates { get; set; }

    public List<SocialMedia> SocialMediaAccounts { get; set; }
    public List<LanguageModel> Languages { get; set; }
}