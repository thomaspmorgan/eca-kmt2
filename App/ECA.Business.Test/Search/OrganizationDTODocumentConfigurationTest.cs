using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;

namespace ECA.Business.Test.Search
{
    [TestClass]
    public class OrganizationDTODocumentConfigurationTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new OrganizationDTODocumentConfiguration();
            Assert.AreEqual(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, instance.DocumentTypeId);
            Assert.AreEqual(OrganizationDTODocumentConfiguration.ORGANIZATION_DOCUMENT_TYPE_NAME, instance.DocumentTypeName);
            Assert.IsNotNull(instance.IdDelegate);
            Assert.IsNotNull(instance.NameDelegate);
            Assert.IsNotNull(instance.DescriptionDelegate);
            Assert.IsNotNull(instance.WebsitesDelegate);
            Assert.IsNotNull(instance.DocumentTypeId);
            Assert.IsNotNull(instance.DocumentTypeName);
            Assert.IsNotNull(instance.AddressesDelegate);

            Assert.IsNull(instance.CountriesDelegate);
            Assert.IsNull(instance.LocationsDelegate);
            Assert.IsNull(instance.StatusDelegate);
            Assert.IsNull(instance.FociDelegate);
            Assert.IsNull(instance.GoalsDelegate);
            Assert.IsNull(instance.OfficeSymbolDelegate);
            Assert.IsNull(instance.PointsOfContactDelegate);
            Assert.IsNull(instance.ThemesDelegate);
            Assert.IsNull(instance.RegionsDelegate);
            Assert.IsNull(instance.StartDateDelegate);
            Assert.IsNull(instance.EndDateDelegate);
            Assert.IsNull(instance.PhoneNumbersDelegate);
        }
    }
}

