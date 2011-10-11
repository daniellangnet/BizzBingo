using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizzBingo.Web.Helper
{
    using System.Text;
    using System.Text.RegularExpressions;

    public static class Extensions
    {
        /// <summary>
        /// Convert Foreign Accent Characters
        /// Thx: http://chrismckee.co.uk/creating-url-slugs-permalinks-in-csharp/
        /// </summary>
        /// <param name="s">string</param>
        /// <returns>Common ASCII representation</returns>
        public static string RemoveAccent(this string s)
        {
            return Encoding.ASCII.GetString(Encoding.GetEncoding("Cyrillic").GetBytes(s));
        }

        /// <summary>
        /// Cleanses String for URL.
        /// Thx: http://chrismckee.co.uk/creating-url-slugs-permalinks-in-csharp/
        /// </summary>
        /// <param name="s">string</param>
        /// <returns>URL Friendly String</returns>
        public static string ToSlug(this string s)
        {
            var sb = new StringBuilder(
                Regex.Replace(
                    HttpUtility.HtmlDecode(s.Replace("&", "and"))
                                            .RemoveAccent(), @"[^\w\ ]", "")
                                            .Trim()
                    );
            sb.Replace("  ", " ").Replace(" ", "-");

            return sb.ToString().ToLower();
        }
    }
}