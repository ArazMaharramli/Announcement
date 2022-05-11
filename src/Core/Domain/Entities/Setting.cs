using System;
using Domain.Common;

namespace Domain.Entities
{
    public class Setting
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
