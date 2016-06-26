using HtmlAgilityPack;
using Medicine.API.Contracts;
using Medicine.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine.API.MedGuideIndia
{
    static class Program
    {
        static void Main()
        {
            string manufacturerUrl = AppSettingsUtility.GetString(AppSettingsKeys.ManufacturerUrl);

            for (int count = 0; count < 53; count++)
            {
                var modifiedManufacturerUrl = manufacturerUrl.Replace("{index}", count.ToString());
                var webRequestHelper = new WebRequestHelper(modifiedManufacturerUrl);
                string response = webRequestHelper.GetResponse();

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(response);

                HtmlNode table = doc.DocumentNode.Descendants("table").FirstOrDefault(u => u.Attributes.Contains("class") && u.Attributes["class"].Value == "tabsborder2");
                if (table != null)
                {
                    var manufacturers = new List<Manufacturer>();
                    var rows = table.SelectNodes("tr").Where(r => r.Attributes.Contains("class") && r.Attributes["class"].Value == "row").ToList();

                    if(rows.Count == 1)
                    {
                        rows = RepairHTML(rows[0].OuterHtml);
                    }

                }

            }


        }

        /// <summary>
        /// Repairs the HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static List<HtmlNode> RepairHTML(string html)
        {
            var rows = new List<HtmlNode>();
            html = html.RemoveDoubleSpaces().Replace("> <", "><");
            html = html.Replace("</a></td><tr", "</a></td></tr><tr");

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml("<html><body><table>" + html + "</table></body></html>");

            rows = doc.DocumentNode.Descendants("table").FirstOrDefault().SelectNodes("tr").ToList();

            rows.RemoveAt(rows.Count - 1);

            return rows;
        }
    }
}
