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

        public class ProjectContact
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Telephone { get; set; }
        }
    }
}