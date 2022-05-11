using System;
using System.Collections.Generic;

namespace Application.Common.Models
{
    public class DataTablePagedList<T> : IDataTablePagedList<T>
    {
        public DataTablePagedList(IEnumerable<T> data, int total, int page, int perpage, string sortColumn, string sortDir)
        {
            Data = data;
            Meta = new DatatableMeta
            {
                Total = total,
                Perpage = perpage,
                Page = page,
                Pages = (int)Math.Ceiling(total / (double)perpage),
                SortColumn = sortColumn,
                SortDirection = sortDir
            };

        }
        //public int PageCount { get; protected set; }

        public DatatableMeta Meta { get; private set; }

        public IEnumerable<T> Data { get; protected set; }
    }
}
