using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;

namespace ECA.Business.Test.Search
{
    [TestClass]
    public class OfficeDTODocumentConfigurationTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new OfficeDTODocumentConfiguration();
            Assert.AreEqual(OfficeDTODocumentConfiguration.OFFICE_DTO_DOCUMENT_TYPE_ID, instance.DocumentTypeId);
            Assert.AreEqual(OfficeDTODocumentConfiguration.OFFICE_DOCUMENT_TYPE_NAME, instance.DocumentTypeName);
            Assert.IsNotNull(instance.IdDelegate);
            Assert.IsNotNull(instance.NameDelegate);
            Assert.IsNotNull(instance.DescriptionDelegate);
            Assert.IsNotNull(instance.PointsOfContactDelegate);
            Assert.IsNotNull(instance.ThemesDelegate);
            Assert.IsNotNull(instance.DocumentTypeId);
            Assert.IsNotNull(instance.DocumentTypeName);
            Assert.IsNotNull(instance.GoalsDelegate);
            Assert.IsNotNull(instance.OfficeSymbolDelegate);

            Assert.IsNull(instance.AddressesDelegate);
            Assert.IsNull(instance.WebsitesDelegate);
            Assert.IsNull(instance.CountriesDelegate);
            Assert.IsNull(instance.LocationsDelegate);
            Assert.IsNull(instance.StatusDelegate);
            Assert.IsNull(instance.FociDelegate);
            Assert.IsNull(instance.RegionsDelegate);
            Assert.IsNull(instance.StartDateDelegate);
            Assert.IsNull(instance.EndDateDelegate);
            Assert.IsNull(instance.PhoneNumbersDelegate);
        }
    }
}
