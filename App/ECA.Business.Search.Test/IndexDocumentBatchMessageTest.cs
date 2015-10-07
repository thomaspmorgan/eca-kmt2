using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace ECA.Business.Search.Test
{
    [TestClass]
    public class IndexDocumentBatchMessageTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var instance = new IndexDocumentBatchMessage();
            Assert.IsNotNull(instance.CreatedDocuments);
            Assert.IsNotNull(instance.DeletedDocuments);
            Assert.IsNotNull(instance.ModifiedDocuments);

            Assert.AreEqual(0, instance.CreatedDocuments.Count());
            Assert.AreEqual(0, instance.DeletedDocuments.Count());
            Assert.AreEqual(0, instance.ModifiedDocuments.Count());
        }

        [TestMethod]
        public void TestHasDocumentsToHandle_NoDocumentsToHandle()
        {
            var instance = new IndexDocumentBatchMessage();
            Assert.AreEqual(0, instance.CreatedDocuments.Count());
            Assert.AreEqual(0, instance.DeletedDocuments.Count());
            Assert.AreEqual(0, instance.ModifiedDocuments.Count());
            Assert.IsFalse(instance.HasDocumentsToHandle());
        }

        [TestMethod]
        public void TestHasDocumentsToHandle_HasCreatedDocuments()
        {
            var instance = new IndexDocumentBatchMessage();
            Assert.AreEqual(0, instance.CreatedDocuments.Count());
            Assert.AreEqual(0, instance.DeletedDocuments.Count());
            Assert.AreEqual(0, instance.ModifiedDocuments.Count());
            Assert.IsFalse(instance.HasDocumentsToHandle());

            instance.CreatedDocuments = new List<string> { new DocumentKey(Guid.NewGuid(), 1).ToString() };
            Assert.IsTrue(instance.HasDocumentsToHandle());
        }

        [TestMethod]
        public void TestHasDocumentsToHandle_HasModifiedDocuments()
        {
            var instance = new IndexDocumentBatchMessage();
            Assert.AreEqual(0, instance.CreatedDocuments.Count());
            Assert.AreEqual(0, instance.DeletedDocuments.Count());
            Assert.AreEqual(0, instance.ModifiedDocuments.Count());
            Assert.IsFalse(instance.HasDocumentsToHandle());

            instance.ModifiedDocuments = new List<string> { new DocumentKey(Guid.NewGuid(), 1).ToString() };
            Assert.IsTrue(instance.HasDocumentsToHandle());
        }

        [TestMethod]
        public void TestHasDocumentsToHandle_HasDeletedDocuments()
        {
            var instance = new IndexDocumentBatchMessage();
            Assert.AreEqual(0, instance.CreatedDocuments.Count());
            Assert.AreEqual(0, instance.DeletedDocuments.Count());
            Assert.AreEqual(0, instance.ModifiedDocuments.Count());
            Assert.IsFalse(instance.HasDocumentsToHandle());

            instance.DeletedDocuments = new List<string> { new DocumentKey(Guid.NewGuid(), 1).ToString() };
            Assert.IsTrue(instance.HasDocumentsToHandle());
        }
    }
}
