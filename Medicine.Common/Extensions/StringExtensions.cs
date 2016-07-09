using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Medicine.Common
{
    /// <summary>
    /// String extension class
    /// </summary>
    public static class StringExtensions
    {
        
        /// <summary>
        /// Strip HTMl Tags
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string StripHtmlTagsRegex(this string source)
        {
            if (source.IndexOf("<style", StringComparison.OrdinalIgnoreCase) >= 1 && source.IndexOf("</style", StringComparison.OrdinalIgnoreCase) > 1)
            {
                string strToRemove = source.Substring(source.IndexOf("<style", StringComparison.OrdinalIgnoreCase), source.IndexOf("</style>", StringComparison.OrdinalIgnoreCase) - source.IndexOf("<style", StringComparison.OrdinalIgnoreCase) + 8);
                source = source.Replace(strToRemove, "");
            }
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        /// <summary>
        /// Strip Text
        /// </summary>
        /// <param name="source"></param>
        /// <param name="noOfChar"></param>
        /// <returns></returns>
        public static string StripText(this string source, int noOfChar)
        {
            source = source.StripHtmlTagsRegex();

            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            if (source.Length > noOfChar)
            {
                source = source.Substring(0, Math.Min(noOfChar, source.Length));
                int end = source.LastIndexOfAny(new char[] { ' ', '\t', '\n', ',' });
                if (end > 0)
                {
                    source = source.Substring(0, end);
                }
                source += "...";
            }
            else
            {
                return source;
            }

            return string.IsNullOrEmpty(source) ? string.Empty : source.Length > noOfChar ? source.Substring(0, Math.Min(noOfChar, source.Length)) + "..." : source;
        }

        /// <summary>
        /// Get Url Friendly Name
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string GetUrlFriendlyName(this string title)
        {
            // make it all lower case
            title = title.ToLower(CultureInfo.CurrentCulture);
            // remove entities
            title = Regex.Replace(title, @"&\w+;", "");
            // remove anything that is not letters, numbers, dash, or space
            title = Regex.Replace(title, @"[^a-z0-9\-\s]", "");
            // replace spaces
            title = title.Replace(' ', '-');
            // collapse dashes
            title = Regex.Replace(title, @"-{2,}", "-");
            // trim excessive dashes at the beginning
            title = title.TrimStart(new[] { '-' });
            // if it's too long, clip it
           
            // remove trailing dashes
            title = title.TrimEnd(new[] { '-' });
            return title;
        }

        /// <summary>
        /// Strip HTML
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string StripHtmlChar(string source)
        {
            if (source.IndexOf("<style", StringComparison.OrdinalIgnoreCase) >= 1 && source.IndexOf("</style", StringComparison.OrdinalIgnoreCase) > 1)
            {
                string strToRemove = source.Substring(source.IndexOf("<style", StringComparison.OrdinalIgnoreCase), source.IndexOf("</style>", StringComparison.OrdinalIgnoreCase) - source.IndexOf("<style", StringComparison.OrdinalIgnoreCase) + 8);
                source = source.Replace(strToRemove, "");
            }
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        /// <summary>
        /// To DateTime
        /// </summary>
        /// <param name="source"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string source, string format)
        {
            try
            {
                CultureInfo enUS = new CultureInfo("en-US");
                DateTime dateValue;
                if (DateTime.TryParseExact(source, format, enUS, DateTimeStyles.None, out dateValue))
                {
                    return dateValue;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Get Time Ago From Datetime
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string TimeAgo(DateTime dt)
        {
            TimeSpan span = DateTime.Now - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("about {0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("about {0} {1} ago",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("about {0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("about {0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("about {0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 5)
                return String.Format("about {0} seconds ago", span.Seconds);
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }

        /// <summary>
        /// decrypt string from base 64
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string DecryptFromBase64(this string source)
        {
            string strRetVal = string.Empty;

            try
            {
                byte[] byt = Convert.FromBase64String(source);
                strRetVal = System.Text.Encoding.UTF8.GetString(byt);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog("StringExtensions", "DecryptFromBase64", ex, "");
            }

            return strRetVal;
        }

        /// <summary>
        /// remove double spaces
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string RemoveDoubleSpaces(this string source)
        {
           return Regex.Replace(source, @"\s+", " ", RegexOptions.Multiline);
        }

        
    }
}
