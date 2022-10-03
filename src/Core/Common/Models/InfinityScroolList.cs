using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class InfinityScroolList<T> where T : class
    {
        public InfinityScroolList(List<T> data, bool hasNext, DateTime startDate, DateTime endDate)
        {
            Data = data;
            HasNext = hasNext;
            StartDate = startDate;
            EndDate = endDate;
        }

        public List<T> Data { get; set; }
        public bool HasNext { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}

