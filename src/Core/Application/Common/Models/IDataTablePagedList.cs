using System.Collections.Generic;

namespace Application.Common.Models
{
    public interface IDataTablePagedList<T>
    {
        //int PageCount { get; }
        DatatableMeta Meta { get; }
        int RecordsTotal { get; }
        int RecordsFiltered { get; }
        IEnumerable<T> Data { get; }
    }
}
