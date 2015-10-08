using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;
using System.Collections.Generic;
using Moq;
using System.IO;

namespace ECA.WebJobs.Search.Index.Test
{
    [TestClass]
    public class FunctionsTest
    {
        private IList<IDocumentService> services;

        [TestInitialize]
        public void TestInit()
        {
            services = new List<IDocumentService>();
        }

        [TestMethod]
        public void TestDispose()
        {
            var service1 = new Mock<IDocumentService>();
            var service2 = new Mock<IDocumentService>();

            var disposableService1 = service1.As<IDisposable>();
            var disposableService2 = service2.As<IDisposable>();
            
            services.Add(service1.Object);
            services.Add(service2.Object);

            var functions = new Functions(services);
            functions.Dispose();
            disposableService1.Verify(x => x.Dispose(), Times.Once());
            disposableService2.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void TestDispose_ServicesNotDisposable()
        {
            var service1 = new Mock<IDocumentService>();
            var service2 = new Mock<IDocumentService>();            
            services.Add(service1.Object);
            services.Add(service2.Object);
            var functions = new Functions(services);
            Action a = () => functions.Dispose();
            a.ShouldNotThrow();
        }

        [TestMethod]
        public void TestHandleCreatedDocuments()
        {
            var documentTypeId = Guid.NewGuid();
            var key = new DocumentKey(documentTypeId, 1);
            var keys = new List<DocumentKey> { key };

            var message = new IndexDocumentBatchMessage();
            message.CreatedDocuments = keys.Select(x => x.ToString()).ToList();

            Action<object> tester = (objectId) =>
            {
                Assert.AreEqual(key.Value, objectId);
            };

            var service = new Mock<IDocumentService>();
            service.Setup(x => x.GetDocumentTypeId()).Returns(documentTypeId);
            service.Setup(x => x.AddOrUpdateDocument(It.IsAny<object>())).Callback(tester);
            services.Add(service.Object);

            var functions = new Functions(services);
            functions.HandleCreatedDocuments(message.CreatedDocuments);
            service.Verify(x => x.AddOrUpdateDocument(It.IsAny<object>()), Times.Once());
        }

        [TestMethod]
        public void TestHandleCreatedDocuments_DocumentTypeIdNotSupported()
        {
            var documentTypeId = Guid.NewGuid();
            var key = new DocumentKey(documentTypeId, 1);
            var keys = new List<DocumentKey> { key };

            var message = new IndexDocumentBatchMessage();
            message.CreatedDocuments = keys.Select(x => x.ToString()).ToList();

            var service = new Mock<IDocumentService>();
            service.Setup(x => x.GetDocumentTypeId()).Returns(Guid.NewGuid());

            var functions = new Functions(services);
            Action a = () => functions.HandleCreatedDocuments(message.CreatedDocuments);
            a.ShouldThrow<NotSupportedException>().WithMessage(String.Format("A service as not found for the document type id [{0}].", documentTypeId));
        }

        [TestMethod]
        public void TestHandleUpdatedDocuments()
        {
            var documentTypeId = Guid.NewGuid();
            var key = new DocumentKey(documentTypeId, 1);
            var keys = new List<DocumentKey> { key };

            Action<object> tester = (objectId) =>
            {
                Assert.AreEqual(key.Value, objectId);
            };

            var service = new Mock<IDocumentService>();
            service.Setup(x => x.GetDocumentTypeId()).Returns(documentTypeId);
            service.Setup(x => x.AddOrUpdateDocument(It.IsAny<object>())).Callback(tester);
            services.Add(service.Object);

            var functions = new Functions(services);
            functions.HandleUpdatedDocuments(keys.Select(x => x.ToString()).ToList());
            service.Verify(x => x.AddOrUpdateDocument(It.IsAny<object>()), Times.Once());
        }

        [TestMethod]
        public void TestHandleUpdatedDocuments_DocumentTypeIdNotSupported()
        {
            var documentTypeId = Guid.NewGuid();
            var key = new DocumentKey(documentTypeId, 1);
            var keys = new List<DocumentKey> { key };

            var service = new Mock<IDocumentService>();
            service.Setup(x => x.GetDocumentTypeId()).Returns(Guid.NewGuid());

            var functions = new Functions(services);
            Action a = () => functions.HandleUpdatedDocuments(keys.Select(x => x.ToString()).ToList());
            a.ShouldThrow<NotSupportedException>().WithMessage(String.Format("A service as not found for the document type id [{0}].", documentTypeId));
        }

        [TestMethod]
        public void TestHandleDeletedDocuments_DocumentTypeIdNotSupported()
        {
            var documentTypeId = Guid.NewGuid();
            var key = new DocumentKey(documentTypeId, 1);
            var keys = new List<DocumentKey> { key };

            var service = new Mock<IDocumentService>();
            service.Setup(x => x.GetDocumentTypeId()).Returns(Guid.NewGuid());

            var functions = new Functions(services);
            Action a = () => functions.HandleDeletedDocuments(keys.Select(x => x.ToString()).ToList());
            a.ShouldThrow<NotSupportedException>().WithMessage(String.Format("A service as not found for the document type id [{0}].", documentTypeId));
        }

        [TestMethod]
        public void TestHandleDeletedDocuments_OneDocumentType()
        {
            var documentTypeId = Guid.NewGuid();
            var key = new DocumentKey(documentTypeId, 1);
            var keys = new List<DocumentKey> { key };

            var service = new Mock<IDocumentService>();
            service.Setup(x => x.GetDocumentTypeId()).Returns(documentTypeId);
            services.Add(service.Object);

            var functions = new Functions(services);
            functions.HandleDeletedDocuments(keys.Select(x => x.ToString()).ToList());
            service.Verify(x => x.DeleteDocuments(It.IsAny<List<object>>()), Times.Once());
        }

        [TestMethod]
        public void TestHandleDeletedDocuments_MultipleDocumentTypes()
        {
            var documentType1 = Guid.NewGuid();
            var key1 = new DocumentKey(documentType1, 1);

            var documentType2 = Guid.NewGuid();
            var key2 = new DocumentKey(documentType2, 2);
            var keys = new List<DocumentKey> { key1, key2 };

            var service1 = new Mock<IDocumentService>();
            service1.Setup(x => x.GetDocumentTypeId()).Returns(documentType1);
            services.Add(service1.Object);

            var service2 = new Mock<IDocumentService>();
            service2.Setup(x => x.GetDocumentTypeId()).Returns(documentType2);
            services.Add(service2.Object);

            var functions = new Functions(services);
            functions.HandleDeletedDocuments(keys.Select(x => x.ToString()).ToList());
            service1.Verify(x => x.DeleteDocuments(It.IsAny<List<object>>()), Times.Once());
            service2.Verify(x => x.DeleteDocuments(It.IsAny<List<object>>()), Times.Once());
        }

        [TestMethod]
        public void TestHandleDeletedDocuments_NoDocuments()
        {
            var documentTypeId = Guid.NewGuid();
            var keys = new List<DocumentKey>();

            var service = new Mock<IDocumentService>();
            service.Setup(x => x.GetDocumentTypeId()).Returns(documentTypeId);
            services.Add(service.Object);

            var functions = new Functions(services);
            functions.HandleDeletedDocuments(keys.Select(x => x.ToString()).ToList());
            service.Verify(x => x.DeleteDocuments(It.IsAny<List<object>>()), Times.Never());
        }

        [TestMethod]
        public void TestProcessQueueMessage_CreatedDocuments()
        {
            var documentTypeId = Guid.NewGuid();
            var key = new DocumentKey(documentTypeId, 1);
            var keys = new List<DocumentKey> { key };
            var keysAsStrings = keys.Select(x => x.ToString()).ToList();

            var service = new Mock<IDocumentService>();
            service.Setup(x => x.GetDocumentTypeId()).Returns(documentTypeId);
            services.Add(service.Object);

            var message = new IndexDocumentBatchMessage();
            message.IsDebugMessage = false;
            message.CreatedDocuments = keysAsStrings;
            var functions = new Functions(services);
            functions.ProcessQueueMessage(message, new Mock<TextWriter>().Object);
            service.Verify(x => x.AddOrUpdateDocument(It.IsAny<object>()), Times.Once());
        }

        [TestMethod]
        public void TestProcessQueueMessage_ModifiedDocuments()
        {
            var documentTypeId = Guid.NewGuid();
            var key = new DocumentKey(documentTypeId, 1);
            var keys = new List<DocumentKey> { key };
            var keysAsStrings = keys.Select(x => x.ToString()).ToList();

            var service = new Mock<IDocumentService>();
            service.Setup(x => x.GetDocumentTypeId()).Returns(documentTypeId);
            services.Add(service.Object);

            var message = new IndexDocumentBatchMessage();
            message.IsDebugMessage = false;
            message.ModifiedDocuments = keysAsStrings;
            var functions = new Functions(services);
            functions.ProcessQueueMessage(message, new Mock<TextWriter>().Object);
            service.Verify(x => x.AddOrUpdateDocument(It.IsAny<object>()), Times.Once());
        }

        [TestMethod]
        public void TestProcessQueueMessage_DeletedDocuments()
        {
            var documentTypeId = Guid.NewGuid();
            var key = new DocumentKey(documentTypeId, 1);
            var keys = new List<DocumentKey> { key };
            var keysAsStrings = keys.Select(x => x.ToString()).ToList();

            var service = new Mock<IDocumentService>();
            service.Setup(x => x.GetDocumentTypeId()).Returns(documentTypeId);
            services.Add(service.Object);

            var message = new IndexDocumentBatchMessage();
            message.IsDebugMessage = false;
            message.DeletedDocuments = keysAsStrings;
            var functions = new Functions(services);
            functions.ProcessQueueMessage(message, new Mock<TextWriter>().Object);
            service.Verify(x => x.DeleteDocuments(It.IsAny<List<object>>()), Times.Once());
        }

        [TestMethod]
        public void TestProcessQueueMessage_IsDebugMessage()
        {
            var documentTypeId = Guid.NewGuid();
            var key = new DocumentKey(documentTypeId, 1);
            var keys = new List<DocumentKey> { key };
            var keysAsStrings = keys.Select(x => x.ToString()).ToList();

            var service = new Mock<IDocumentService>();
            service.Setup(x => x.GetDocumentTypeId()).Returns(documentTypeId);
            services.Add(service.Object);

            var message = new IndexDocumentBatchMessage();
            message.IsDebugMessage = true;
            message.DeletedDocuments = keysAsStrings;
            message.CreatedDocuments = keysAsStrings;
            message.ModifiedDocuments = keysAsStrings;
            var functions = new Functions(services);
            functions.ProcessQueueMessage(message, new Mock<TextWriter>().Object);
            service.Verify(x => x.DeleteDocuments(It.IsAny<List<object>>()), Times.Never());
            service.Verify(x => x.AddOrUpdateDocument(It.IsAny<List<object>>()), Times.Never());
        }

    }
}
