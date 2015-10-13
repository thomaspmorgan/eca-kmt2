using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Search;
using Moq;
using System.Reflection;

namespace ECA.Business.Test.Search
{
    [TestClass]
    public class OfficeDocumentServiceTest
    {
        private InMemoryEcaContext context;
        private OfficeDocumentService service;
        private Mock<IIndexService> indexService;
        private Mock<IIndexNotificationService> notificationService;
        private int batchSize;


        [TestInitialize]
        public void TestInit()
        {
            batchSize = 1;
            context = new InMemoryEcaContext();
            indexService = new Mock<IIndexService>();
            notificationService = new Mock<IIndexNotificationService>();
            service = new OfficeDocumentService(context, indexService.Object, notificationService.Object, batchSize);
        }


        [TestMethod]
        public void TestConstructor()
        {
            batchSize = 1;
            service = new OfficeDocumentService(context, indexService.Object, notificationService.Object, batchSize);
            Assert.AreEqual(batchSize, service.GetBatchSize());

            var batchSizeField = typeof(OfficeDocumentService).BaseType.GetField("batchSize", BindingFlags.Instance | BindingFlags.NonPublic);
            var batchSizeValue = batchSizeField.GetValue(service);
            Assert.AreEqual(batchSize, batchSizeValue);
        }

        [TestMethod]
        public void TestCreateGetDocumentsQuery()
        {
            var query = service.CreateGetDocumentsQuery();
            Assert.IsNotNull(query);
        }

        [TestMethod]
        public void TestCreateGetDocumentByIdQuery()
        {
            var query = service.CreateGetDocumentByIdQuery(1);
            Assert.IsNotNull(query);
        }
    }
}
