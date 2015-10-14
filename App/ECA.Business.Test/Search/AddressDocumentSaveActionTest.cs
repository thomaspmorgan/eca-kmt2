using System;
using System.Linq;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;
using System.Collections.Specialized;
using ECA.Core.Settings;
using System.Configuration;
using ECA.Data;

namespace ECA.Business.Test.Search
{
    [TestClass]
    public class AddressDocumentSaveActionTest
    {
        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AppSettings settings;
        private AddressDocumentSaveAction saveAction;


        [TestInitialize]
        public void TestInit()
        {
            appSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            settings = new AppSettings(appSettings, connectionStrings);
            saveAction = new AddressDocumentSaveAction(settings);
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
        public void TestGetDocumentKey_HasOrganizationId()
        {
            var orgId = 10;
            var address = new Address
            {
                OrganizationId = orgId
            };
            var expectedKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
            var testKey = saveAction.GetDocumentKey(address);
            Assert.AreEqual(expectedKey, testKey);
        }
        [TestMethod]
        public void TestGetDocumentKey_HasOrganizationReference()
        {
            var orgId = 10;
            var address = new Address
            {
                Organization = new Organization
                {
                    OrganizationId = orgId
                }
            };
            var expectedKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
            var testKey = saveAction.GetDocumentKey(address);
            Assert.AreEqual(expectedKey, testKey);
        }

        [TestMethod]
        public void TestGetDocumentKey_IsNotOrganizationAddress()
        {
            var personId = 10;
            var address = new Address
            {
                PersonId = personId
               
            };
            Action a = () => saveAction.GetDocumentKey(address);
            a.ShouldThrow<NotSupportedException>().WithMessage("Currently people are not indexed for searching; therefore, addresses related to people should not be indexed.  These address should be excluded.");
        }

        [TestMethod]
        public void TestGetBatchMessage_CreatedEntity()
        {
            var orgId = 10;
            var address = new Address
            {
                OrganizationId = orgId
            };

            saveAction.CreatedEntities.Add(address);

            var message = saveAction.GetBatchMessage();
            Assert.AreEqual(0, message.CreatedDocuments.Count());
            Assert.AreEqual(1, message.ModifiedDocuments.Count());
            Assert.AreEqual(0, message.DeletedDocuments.Count());

            var expectedKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
            Assert.AreEqual(expectedKey.ToString(), message.ModifiedDocuments.First());
        }

        [TestMethod]
        public void TestGetBatchMessage_DeletedEntity()
        {
            var orgId = 10;
            var address = new Address
            {
                OrganizationId = orgId
            };

            saveAction.DeletedEntities.Add(address);

            var message = saveAction.GetBatchMessage();
            Assert.AreEqual(0, message.CreatedDocuments.Count());
            Assert.AreEqual(1, message.ModifiedDocuments.Count());
            Assert.AreEqual(0, message.DeletedDocuments.Count());

            var expectedKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
            Assert.AreEqual(expectedKey.ToString(), message.ModifiedDocuments.First());
        }

        [TestMethod]
        public void TestGetBatchMessage_ModifiedEntity()
        {
            var orgId = 10;
            var address = new Address
            {
                OrganizationId = orgId
            };

            saveAction.ModifiedEntities.Add(address);

            var message = saveAction.GetBatchMessage();
            Assert.AreEqual(0, message.CreatedDocuments.Count());
            Assert.AreEqual(1, message.ModifiedDocuments.Count());
            Assert.AreEqual(0, message.DeletedDocuments.Count());

            var expectedKey = new DocumentKey(OrganizationDTODocumentConfiguration.ORGANIZATION_DTO_DOCUMENT_TYPE_ID, orgId);
            Assert.AreEqual(expectedKey.ToString(), message.ModifiedDocuments.First());
        }
    }
}
