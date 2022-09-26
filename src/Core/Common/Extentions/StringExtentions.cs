using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

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
            str = str.ToLower();

            Encoding iso = Encoding.GetEncoding(Encoding.Latin1.CodePage);
            Encoding utf8 = Encoding.UTF8;

            byte[] utfBytes = utf8.GetBytes(str);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);

            str = iso.GetString(isoBytes);
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;

        }
    }
}
