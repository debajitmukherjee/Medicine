using HtmlAgilityPack;
using Medicine.API.Contracts;
using Medicine.API.Repository.Manufacturer;
using Medicine.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine.API.MedGuideIndia
{
    public static class Program
    {
        private static readonly IManufacturerRepository manufaturerRepository = new ManufacturerRepository();

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        static void Main()
        {
            string manufacturerUrl = AppSettingsUtility.GetString(AppSettingsKeys.ManufacturerUrl);

            for (int count = 0; count < 53; count++)
            {
                var modifiedManufacturerUrl = manufacturerUrl.Replace("{index}", count.ToString());
                var webRequestHelper = new WebRequestHelper(modifiedManufacturerUrl);
                string response = webRequestHelper.GetResponse();

                var manufacturers = GetManufaturersFromHtml(response);

                // Add to database
                if(manufacturers.Count > 0)
                {
                    manufaturerRepository.Add(manufacturers);
                }

                Console.Clear();
                Console.Write("Manufacturer Updated: " + count * 50);
            }

        }        

        /// <summary>
        /// Gets the manufaturers from HTML.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static List<Manufacturer> GetManufaturersFromHtml(string html)
        {
            List<Manufacturer> manufacturers = new List<Manufacturer>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            HtmlNode table = doc.DocumentNode.Descendants("table").FirstOrDefault(u => u.Attributes.Contains("class") && u.Attributes["class"].Value == "tabsborder2");
            if (table != null)
            {

                var rows = table.SelectNodes("tr").Where(r => r.Attributes.Contains("class") && r.Attributes["class"].Value == "row").ToList();

                if (rows.Count == 1)
                {
                    rows = RepairHtml(rows[0].OuterHtml);
                    manufacturers = new List<Manufacturer>();
                    foreach (var row in rows)
                    {
                        var manufaturer = GetManufacturer(row);
                        if (manufaturer != null)
                        {
                            manufacturers.Add(manufaturer);
                        }
                    }


                }

            }

            return manufacturers;
        }

        /// <summary>
        /// Gets the manufacturer.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        private static Manufacturer GetManufacturer(HtmlNode row)
        {
            var cells = row.SelectNodes("td");
            if (cells.Count == 9)
            {
                var name = cells[1].InnerHtml;
                if (name.IndexOf(")", StringComparison.OrdinalIgnoreCase) > 1 && name.IndexOf("(", StringComparison.OrdinalIgnoreCase) < 0)
                {
                    name = name.Replace(")", "");
                }

                int medGuideId = 0;
                var anchor = cells[8].SelectNodes("a").FirstOrDefault();
                if (anchor != null)
                {
                    int.TryParse(anchor.Attributes["href"].Value.ToLower(AppSettingsKeys.CurrentCulture).Replace("manufact_brand.php?comp_id=", ""), out medGuideId);
                }

                if (medGuideId > 0)
                {
                    Manufacturer manufaturer = new Manufacturer
                    {
                        Name = name.Trim(),
                        Address = cells[2].InnerHtml.Trim(),
                        PhoneNo = cells[3].InnerHtml.Trim(),
                        Fax = cells[4].InnerHtml.Trim(),
                        Url = cells[6].InnerHtml.Trim(),
                        MedGuideRefId = medGuideId
                    };
                    return manufaturer;
                }

                return null;
            }
            return null;
        }

        /// <summary>
        /// Repairs the Html.
        /// </summary>
        /// <param name="html">The Html.</param>
        /// <returns></returns>
        private static List<HtmlNode> RepairHtml(string html)
        {
            html = html.RemoveDoubleSpaces().Replace("> <", "><");
            html = html.Replace("</a></td><tr", "</a></td></tr><tr");

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml("<html><body><table>" + html + "</table></body></html>");

            var rows = doc.DocumentNode.Descendants("table").FirstOrDefault().SelectNodes("tr").ToList();

            rows.RemoveAt(rows.Count - 1);

            return rows;
        }
    }
}
