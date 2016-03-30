using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;
using System.Collections.Specialized;
using ECA.Core.Settings;
using System.Configuration;
using ECA.Data;
using Microsoft.QualityTools.Testing.Fakes;
using ECA.Core.DynamicLinq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace ECA.Business.Test.Search
{
    [TestClass]
    public class AddressToEntityDocumentSaveActionTest
    {
        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AppSettings settings;
        private AddressToEntityDocumentSaveAction saveAction;
        private InMemoryEcaContext context;


        [TestInitialize]
        public void TestInit()
        {
            context = new InMemoryEcaContext();
            appSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            settings = new AppSettings(appSettings, connectionStrings);
            saveAction = new AddressToEntityDocumentSaveAction(settings);
        }


        [TestMethod]
        public void TestIsCreatedEntityExcluded()
        {
            Assert.IsFalse(saveAction.IsCreatedEntityExcluded(new Address()));
        }

        [TestMethod]
        public void TestIsModifiedEntityExcluded()
        {
            Assert.IsFalse(saveAction.IsModifiedEntityExcluded(new Address()));
        }

        [TestMethod]
        public void TestIsDeletedEntityExcluded()
        {
            Assert.IsFalse(saveAction.IsDeletedEntityExcluded(new Address()));
        }

        [TestMethod]
        public async Task TestGetOrganizationDocumentKeys_DbPropertyValues_HasOrgId()
        {
            using (ShimsContext.Create())
            {
                var orgId = 10;
                var address = new Address
                {
                    OrganizationId = orgId
                };
                var dbPropertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                dbPropertyValues.GetValueOf1String<int?>((propertyName) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId), propertyName);
                    return orgId;
                });
                Action<List<DocumentKey>> tester = (documentKeys) =>
                {
                    Assert.AreEqual(1, documentKeys.Count);
                    var expectedDocumentKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
                    Assert.AreEqual(expectedDocumentKey, documentKeys.First());
                };
                var keys = saveAction.GetRelatedEntityDocumentKeysOfModifiedEntity(address, dbPropertyValues);
                var keysAsync = await saveAction.GetRelatedEntityDocumentKeysOfModifiedEntityAsync(address, dbPropertyValues);
                tester(keys);
                tester(keysAsync);
            }
        }

        [TestMethod]
        public async Task TestGetOrganizationDocumentKeys_DbPropertyValues_DoesNotHaveOrgId()
        {
            using (ShimsContext.Create())
            {
                var dbPropertyValues = new System.Data.Entity.Infrastructure.Fakes.ShimDbPropertyValues();
                dbPropertyValues.GetValueOf1String<int?>((propertyName) =>
                {
                    Assert.AreEqual(PropertyHelper.GetPropertyName<Address>(x => x.OrganizationId), propertyName);
                    return null;
                });
                Action<List<DocumentKey>> tester = (documentKeys) =>
                {
                    Assert.AreEqual(0, documentKeys.Count);
                };
                var keys = saveAction.GetRelatedEntityDocumentKeysOfModifiedEntity(null, dbPropertyValues);
                var keysAsync = await saveAction.GetRelatedEntityDocumentKeysOfModifiedEntityAsync(null, dbPropertyValues);
                tester(keys);
                tester(keysAsync);
            }
        }

        [TestMethod]
        public void TestGetOrganizationDocumentKeys_DoesNotHaveOrgIdOrOrgReference()
        {
            var address = new Address();
            Assert.IsFalse(address.OrganizationId.HasValue);
            Assert.IsNull(address.Organization);
            var keys = saveAction.GetOrganizationDocumentKeys(address);
            Assert.AreEqual(0, keys.Count);
        }

        [TestMethod]
        public void TestGetOrganizationDocumentKeys_HasOrganizationId()
        {
            var orgId = 10;
            var address = new Address
            {
                OrganizationId = orgId
            };
            var expectedDocumentKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
            var documentKeys = saveAction.GetOrganizationDocumentKeys(address);
            Assert.AreEqual(1, documentKeys.Count);
            Assert.AreEqual(expectedDocumentKey, documentKeys.First());
        }

        [TestMethod]
        public void TestGetOrganizationDocumentKeys_HasOrganizationReference()
        {
            var orgId = 10;
            var address = new Address
            {
                Organization = new Organization
                {
                    OrganizationId = orgId
                }
            };
            var expectedDocumentKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
            var documentKeys = saveAction.GetOrganizationDocumentKeys(address);
            Assert.AreEqual(1, documentKeys.Count);
            Assert.AreEqual(expectedDocumentKey, documentKeys.First());
        }

        [TestMethod]
        public async Task TestGetRelatedEntityDocumentKeysOfAddedEntity()
        {
            var orgId = 10;
            var address = new Address
            {
                OrganizationId = orgId
            };
            Action<List<DocumentKey>> tester = (documentKeys) =>
            {
                Assert.AreEqual(1, documentKeys.Count);
                var expectedDocumentKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
                Assert.AreEqual(expectedDocumentKey, documentKeys.First());
            };
            var keys = saveAction.GetRelatedEntityDocumentKeysOfAddedEntity(address);
            var keysAsync = await saveAction.GetRelatedEntityDocumentKeysOfAddedEntityAsync(address);
            tester(keys);
            tester(keysAsync);
        }
    }
}
