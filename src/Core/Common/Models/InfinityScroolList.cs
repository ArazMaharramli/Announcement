using System;
using System.Collections.Generic;

namespace Common.Models
{
    public class InfinityScroolList<T> where T : class
    {
        public InfinityScroolList(List<T> data, bool hasNext)
        {
            Data = data;
            HasNext = hasNext;
        }

        public List<T> Data { get; set; }
        public bool HasNext { get; set; }
    }
}

