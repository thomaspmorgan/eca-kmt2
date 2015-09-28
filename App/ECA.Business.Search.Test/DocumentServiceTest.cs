using System;
using FluentAssertions;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Azure.Search.Models;

namespace ECA.Business.Search.Test
{

    public class TestDocumentService : DocumentService<TestContext, SimpleEntity>
    {

        public TestDocumentService(TestContext context, IIndexService indexService, IIndexNotificationService notificationService, int batchSize = 500) : base(context, indexService, notificationService, batchSize)
        {
        }

     

        public TestContext GetContext()
        {
            return this.Context;
        }

        public override IQueryable<SimpleEntity> CreateGetDocumentsQuery()
        {
            return this.Context.SimpleEntities.Select(x => x);
        }
    }

    [TestClass]
    public class DocumentServiceTest
    {
        private int batchSize;
        private TestContext context;
        private TestDocumentService service;
        private Mock<IIndexService> indexService;
        private Mock<IIndexNotificationService> notificationService;

        [TestInitialize]
        public void TestInit()
        {
            batchSize = 1;
            indexService = new Mock<IIndexService>();
            notificationService = new Mock<IIndexNotificationService>();
            context = new TestContext();
            service = new TestDocumentService(context, indexService.Object, notificationService.Object, batchSize);
        }

        [TestMethod]
        public void TestConstructor_DefaultBatchSize()
        {
            var testService = new TestDocumentService(context, indexService.Object, notificationService.Object);
            Assert.IsNotNull(testService.GetContext());
            Assert.IsFalse(testService.GetContext().Configuration.AutoDetectChangesEnabled);

            var batchSizeField = typeof(TestDocumentService).BaseType.GetField("batchSize", BindingFlags.Instance | BindingFlags.NonPublic);
            var batchSizeValue = batchSizeField.GetValue(testService);
            Assert.AreEqual(TestDocumentService.DEFAULT_BATCH_SIZE, batchSizeValue);
        }

        [TestMethod]
        public void TestConstructor()
        {
            var batchSize = 10;
            var testService = new TestDocumentService(context, indexService.Object, notificationService.Object, batchSize);
            Assert.IsNotNull(testService.GetContext());
            Assert.IsFalse(testService.GetContext().Configuration.AutoDetectChangesEnabled);

            var batchSizeField = typeof(TestDocumentService).BaseType.GetField("batchSize", BindingFlags.Instance | BindingFlags.NonPublic);
            var batchSizeValue = batchSizeField.GetValue(testService);
            Assert.AreEqual(batchSize, batchSizeValue);
        }

        [TestMethod]
        public async Task TestGetDocumentCount()
        {
            var instance = new SimpleEntity();
            context.SimpleEntities.Add(instance);

            Action<int> tester = (serviceResult) =>
            {
                Assert.AreEqual(context.SimpleEntities.Count(), serviceResult);
            };

            var count = service.GetDocumentCount();
            var countAsync = await service.GetDocumentCountAsync();
            tester(count);
            tester(countAsync);
        }

        [TestMethod]
        public async Task TestGetDocumentBatch()
        {
            Assert.AreEqual(1, batchSize);
            var instance1 = new SimpleEntity();
            var instance2 = new SimpleEntity();
            context.SimpleEntities.Add(instance1);
            context.SimpleEntities.Add(instance2);
            var totalCount = context.SimpleEntities.Count();

            Action<List<SimpleEntity>, SimpleEntity> tester = (serviceResults, expectedSimpleEntity) =>
            {
                Assert.AreEqual(1, serviceResults.Count);
                Assert.IsTrue(Object.ReferenceEquals(serviceResults.First(), expectedSimpleEntity));
            };
            var counter = 0;
            while(counter < totalCount)
            {
                var expectedEntity = context.SimpleEntities.ToList()[counter];
                var batch = service.GetDocumentBatch(counter, batchSize);
                var batchAsync = await service.GetDocumentBatchAsync(counter, batchSize);
                tester(batch, expectedEntity);
                tester(batchAsync, expectedEntity);
                counter += batchSize;
            }
        }

        [TestMethod]
        public async Task TestProcess()
        {
            Assert.AreEqual(1, batchSize);
            var instance1 = new SimpleEntity();
            var instance2 = new SimpleEntity();
            context.SimpleEntities.Add(instance1);
            context.SimpleEntities.Add(instance2);
            var totalCount = context.SimpleEntities.Count();

            var counter = 0;
            var counterAsync = 0;
            Action<List<SimpleEntity>> documentsToHandleCallback = (docs) =>
            {   
                Assert.AreEqual(1, docs.Count);
                Assert.IsTrue(Object.ReferenceEquals(docs.First(), context.SimpleEntities.ToList()[counter]));
                counter++;
            };
            Action<List<SimpleEntity>> documentsToHandleAsyncCallback = (docs) =>
            {
                Assert.AreEqual(1, docs.Count);
                Assert.IsTrue(Object.ReferenceEquals(docs.First(), context.SimpleEntities.ToList()[counterAsync]));
                counterAsync++;
            };

            indexService.Setup(x => x.GetDocumentConfiguration<SimpleEntity>())
                .Returns(new SimpleEntityConfiguration());
            indexService.Setup(x => x.HandleDocuments<SimpleEntity>(It.IsAny<List<SimpleEntity>>()))
                .Callback(documentsToHandleCallback);
            indexService.Setup(x => x.HandleDocumentsAsync<SimpleEntity>(It.IsAny<List<SimpleEntity>>()))
                .ReturnsAsync(new DocumentIndexResponse())
                .Callback(documentsToHandleAsyncCallback);
            
            service.Process();
            await service.ProcessAsync();
            notificationService.Verify(x => x.Started(It.IsAny<DocumentType>()), Times.Exactly(2));
            notificationService.Verify(x => x.Processed(It.IsAny<DocumentType>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(4));
            notificationService.Verify(x => x.Finished(It.IsAny<DocumentType>()), Times.Exactly(2));
            indexService.Verify(x => x.CreateIndex<SimpleEntity>(), Times.Once());
            indexService.Verify(x => x.CreateIndexAsync<SimpleEntity>(), Times.Once());
        }

        [TestMethod]
        public async Task TestProcess_DocumentConfigurationDoesNotExist()
        {
            SimpleEntityConfiguration configuration = null;
            indexService.Setup(x => x.GetDocumentConfiguration<SimpleEntity>()).Returns(configuration);
            var message = String.Format("The document configuration for the type [{0}] was not found.", typeof(SimpleEntity));

            Action a = () =>
            {
                service.Process();
            };
            Func<Task> f = () =>
            {
                return service.ProcessAsync();
            };
            a.ShouldThrow<NotSupportedException>().WithMessage(message);
            f.ShouldThrow<NotSupportedException>().WithMessage(message);
        }
    }
}
