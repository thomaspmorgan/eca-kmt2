using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class OfficeSettingsTest
    {
        [TestMethod]
        public void TestToString_NoSettings()
        {
            var officeSettings = new OfficeSettings();
            Assert.IsNotNull(officeSettings.ToString());
        }

        [TestMethod]
        public void TestToString_HasSettings()
        {
            var officeSettings = new OfficeSettings();
            officeSettings.CategoryLabel = "cat";
            officeSettings.FocusLabel = "foc";
            officeSettings.IsCategoryRequired = true;
            officeSettings.IsObjectiveRequired = true;
            officeSettings.JustificationLabel = "just";
            officeSettings.MaximumRequiredFoci = 1;
            officeSettings.MinimumRequiredFoci = 1;
            officeSettings.ObjectiveLabel = "obj";
            Assert.IsNotNull(officeSettings.ToString());
        }
    }
}
