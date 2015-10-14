using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using System.Configuration;
using ECA.Core.Settings;
using ECA.Business.Search;
using ECA.Data;
using System.Reflection;
using ECA.Core.Generation;

namespace ECA.Business.Test.Search
{
    [TestClass]
    public class OrganizationDocumentSaveActionTest
    {
        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AppSettings settings;
        private OrganizationDocumentSaveAction saveAction;

        [TestInitialize]
        public void TestInit()
        {
            appSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            settings = new AppSettings(appSettings, connectionStrings);
            saveAction = new OrganizationDocumentSaveAction(settings);
        }

        [TestMethod]
        public void TestGetDocumentKey()
        {
            var documentTypeId = OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID;
            var office = new Organization
            {
                OrganizationId = 10
            };
            var expectedKey = new DocumentKey(documentTypeId, office.OrganizationId);
            var testKey = saveAction.GetDocumentKey(office);
            Assert.AreEqual(expectedKey, testKey);
        }

        [TestMethod]
        public void TestIsCreatedEntityExcluded()
        {
            var officeTypeIds = Organization.OFFICE_ORGANIZATION_TYPE_IDS;
            var staticProperties = typeof(OrganizationType).GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allStaticLookups = staticProperties.Select(x => x.GetValue(null) as StaticLookup).ToList();
            foreach (var orgType in allStaticLookups)
            {
                var isExpectedToBeExcluded = false;
                if (officeTypeIds.Contains(orgType.Id))
                {
                    isExpectedToBeExcluded = true;
                }
                var testOrganization = new Organization
                {
                    OrganizationTypeId = orgType.Id
                };
                Assert.AreEqual(isExpectedToBeExcluded, saveAction.IsCreatedEntityExcluded(testOrganization));
            }
        }

        [TestMethod]
        public void TestIsModifiedEntityExcluded()
        {
            var officeTypeIds = Organization.OFFICE_ORGANIZATION_TYPE_IDS;
            var staticProperties = typeof(OrganizationType).GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allStaticLookups = staticProperties.Select(x => x.GetValue(null) as StaticLookup).ToList();
            foreach (var orgType in allStaticLookups)
            {
                var isExpectedToBeExcluded = false;
                if (officeTypeIds.Contains(orgType.Id))
                {
                    isExpectedToBeExcluded = true;
                }
                var testOrganization = new Organization
                {
                    OrganizationTypeId = orgType.Id
                };
                Assert.AreEqual(isExpectedToBeExcluded, saveAction.IsModifiedEntityExcluded(testOrganization));
            }
        }

        [TestMethod]
        public void TestIsDeletedEntityExcluded()
        {
            var officeTypeIds = Organization.OFFICE_ORGANIZATION_TYPE_IDS;
            var staticProperties = typeof(OrganizationType).GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allStaticLookups = staticProperties.Select(x => x.GetValue(null) as StaticLookup).ToList();
            foreach (var orgType in allStaticLookups)
            {
                var isExpectedToBeExcluded = false;
                if (officeTypeIds.Contains(orgType.Id))
                {
                    isExpectedToBeExcluded = true;
                }
                var testOrganization = new Organization
                {
                    OrganizationTypeId = orgType.Id
                };
                Assert.AreEqual(isExpectedToBeExcluded, saveAction.IsDeletedEntityExcluded(testOrganization));
            }
        }
    }
}
