using System.Collections.Generic;

namespace Application.Common.Models.ConfigModels
{
    public class TenantInfo
    {
        public string Domain { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public string FaviconIco { get; set; }
        public string Description { get; set; }
        public ProjectContact Contact { get; set; }
        public List<SocialMedia> SocialMediaAccounts { get; set; }
        public class ProjectContact
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Telephone { get; set; }
        }
    }

    public class SocialMedia
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public byte Order { get; set; }
    }
}