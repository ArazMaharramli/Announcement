using System;
using System.Collections.Generic;

namespace Common.Extentions
{
    public static class StringExtentions
    {
        public static string RemoveSymbols(this string value)
        {
            var _remove = new List<string> {
                    "&quot;","&amp;"," ", ",", ".", "/", "?", "!", "&",
                    "\'", "\"", "@", "#", "$", ";", ":",
                    "-", "=", "+", ")", "(", "*", "ˆ", "%", "¿",
                    ">","<","¯","˜","`","Æ","{","}","”","’","±","—",
                    "·","°","‡","ﬂ","ﬁ","›","‹","€","⁄"
                };
            _remove.ForEach(s => value = value?.Replace(s, string.Empty));
            return value.ToUpper();
        }

        public static string ToUrlSlug(this string str)
        {
            return str;
        }
    }
}
