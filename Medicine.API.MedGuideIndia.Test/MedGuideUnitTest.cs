using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Medicine.Common;

namespace Medicine.API.MedGuideIndia.Test
{
    [TestClass]
    public class MedGuideUnitTest
    {
        [TestMethod]
        public void CheckManufacturerResponse()
        {
            string manufacturerUrl = AppSettingsUtility.GetString(AppSettingsKeys.ManufacturerUrl);
            var modifiedManufacturerUrl = manufacturerUrl.Replace("{index}", "0");
            var webRequestHelper = new WebRequestHelper(modifiedManufacturerUrl);
            string response = webRequestHelper.GetResponse();

            Assert.AreNotEqual<string>(response, string.Empty);

            var manufacturers = Program.GetManufaturersFromHtml(response);

            //Manufacturer count should be 50 for first page
            Assert.AreEqual<int>(manufacturers.Count, 50);

        }
    }
}
