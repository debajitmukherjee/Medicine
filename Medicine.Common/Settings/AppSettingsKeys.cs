using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medicine.Common
{
    /// <summary>
    /// This class is used to store app settings keys
    /// </summary>
    public static class AppSettingsKeys
    {
        public static readonly string ManufacturerUrl = "ManufacturerUrl";

        public static readonly string ManufacturerDetailsUrl = "ManufacturerDetailsUrl";

        public static readonly CultureInfo CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
    }
}
