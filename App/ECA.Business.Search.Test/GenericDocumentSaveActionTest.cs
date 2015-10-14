using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Specialized;
using System.Configuration;
using ECA.Core.Settings;

namespace ECA.Business.Search.Test
{
    [TestClass]
    public class GenericDocumentSaveActionTest
    {
        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AppSettings settings;
        private TestContext context;
        private GenericDocumentSaveAction<SimpleEntity> saveAction;

        [TestInitialize]
        public void TestInit()
        {
            appSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            settings = new AppSettings(appSettings, connectionStrings);
            context = new TestContext();
            saveAction = new GenericDocumentSaveAction<SimpleEntity>(settings, SimpleEntityConfiguration.SIMPLE_ENTITY_DOCUMENT_TYPE_ID, x => x.Id);
        }

        [TestMethod]
        public void TestGetDocumentKey()
        {
            var instance = new SimpleEntity();
            instance.Id = 10;
            var expectedKey = new DocumentKey(SimpleEntityConfiguration.SIMPLE_ENTITY_DOCUMENT_TYPE_ID, instance.Id);
            Assert.AreEqual(expectedKey, saveAction.GetDocumentKey(instance, null));
        }

        [TestMethod]
        public void TestIsCreatedDocumentExcluded()
        {
            Assert.IsFalse(saveAction.IsCreatedEntityExcluded(new SimpleEntity()));
        }

        [TestMethod]
        public void TestIsModifiedDocumentExcluded()
        {
            Assert.IsFalse(saveAction.IsModifiedEntityExcluded(new SimpleEntity()));
        }

        [TestMethod]
        public void TestIsDeletedDocumentExcluded()
        {
            Assert.IsFalse(saveAction.IsDeletedEntityExcluded(new SimpleEntity()));
        }
    }
}
