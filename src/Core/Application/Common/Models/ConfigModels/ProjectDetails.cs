namespace Application.Common.Models.ConfigModels
{
    public class ProjectDetails
    {
        public string Name { get; set; }
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