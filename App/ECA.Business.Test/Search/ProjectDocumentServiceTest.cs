using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using Microsoft.Azure.Search;
using ECA.Business.Search;
using System.Collections.Generic;
using ECA.Data;
using Moq;
using System.Reflection;

namespace ECA.Business.Test.Search
{
    [TestClass]
    public class ProjectDocumentServiceTest
    {
        private InMemoryEcaContext context;
        private ProjectDocumentService service;
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
            service = new ProjectDocumentService(context, indexService.Object, notificationService.Object, batchSize);
        }


        [TestMethod]
        public void TestConstructor()
        {
            batchSize = 1;
            service = new ProjectDocumentService(context, indexService.Object, notificationService.Object, batchSize);
            Assert.AreEqual(batchSize, service.GetBatchSize());

            var batchSizeField = typeof(ProjectDocumentService).BaseType.GetField("batchSize", BindingFlags.Instance | BindingFlags.NonPublic);
            var batchSizeValue = batchSizeField.GetValue(service);
            Assert.AreEqual(batchSize, batchSizeValue);
        }

        [TestMethod]
        public void TestCreateGetDocumentsQuery()
        {
            var query = service.CreateGetDocumentsQuery();
            Assert.IsNotNull(query);
        }
    }
}
