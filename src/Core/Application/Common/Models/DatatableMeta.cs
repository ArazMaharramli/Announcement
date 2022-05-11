namespace Application.Common.Models
{
    public class DatatableMeta
    {
        public int Pages { get; set; }
        public int Page { get; set; }
        public int Perpage { get; set; }
        public int Total { get; set; }
        public string SortDirection { get; set; }
        public string SortColumn { get; set; }
    }
}
