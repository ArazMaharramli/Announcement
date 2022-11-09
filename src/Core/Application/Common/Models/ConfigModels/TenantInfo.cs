using System.Collections.Generic;

namespace Application.Common.Models.ConfigModels
{
    public class TenantInfo
    {
        public string Domain { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string Icon { get; set; }
        public string FaviconIco { get; set; }
        public string ShortDescription { get; set; }
        public string Slogan { get; set; }

        public ProjectContact Contact { get; set; }
        public List<SocialMedia> SocialMediaAccounts { get; set; }

        public List<LanguageModel> Languages { get; set; }
    }

    public class ProjectContact
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
    }

    public class SocialMedia
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public byte Order { get; set; }
    }

    public class LanguageModel
    {
        public string DisplayName { get; set; }
        public string CountryCode { get; set; }
        public string Culture { get; set; }
        public string Flag { get; set; }
        public bool IsDefault { get; set; }
    }
}