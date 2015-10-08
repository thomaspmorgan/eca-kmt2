using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ECA.Business.Search;
using Moq;
using System.IO;
using System.Collections.Specialized;
using System.Configuration;
using ECA.Core.Settings;

namespace ECA.WebJobs.Search.Index.All.Test
{
    [TestClass]
    public class FunctionsTest
    {
        private NameValueCollection appSettings;
        private ConnectionStringSettingsCollection connectionStrings;
        private AppSettings settings;

        [TestInitialize]
        public void TestInit()
        {
            appSettings = new NameValueCollection();
            connectionStrings = new ConnectionStringSettingsCollection();
            settings = new AppSettings(appSettings, connectionStrings);
            appSettings.Add(AppSettings.SEARCH_INDEX_NAME_KEY, "value");
        }

        [TestMethod]
        public void TestIndex_NothingIsDisposable()
        {
            var indexService = new Mock<IIndexService>();

            var documentServices = new List<IDocumentService>();
            var firstService = new Mock<IDocumentService>();

            var secondService = new Mock<IDocumentService>();

            documentServices.Add(firstService.Object);
            documentServices.Add(secondService.Object);

            var writer = new Mock<TextWriter>();
            var functions = new Functions();
            functions.Index(documentServices, indexService.Object, settings);
            indexService.Verify(x => x.DeleteIndex(It.IsAny<string>()), Times.Once());

            firstService.Verify(x => x.AddOrUpdateAll(), Times.Once());
            secondService.Verify(x => x.AddOrUpdateAll(), Times.Once());            
        }

        [TestMethod]
        public void TestIndex()
        {
            var indexService = new Mock<IIndexService>();
            var disposableIndexService = indexService.As<IDisposable>();

            var documentServices = new List<IDocumentService>();
            var firstService = new Mock<IDocumentService>();
            var firstDisposableService = firstService.As<IDisposable>();

            var secondService = new Mock<IDocumentService>();
            var secondDisposableService = firstService.As<IDisposable>();

            documentServices.Add(firstService.Object);
            documentServices.Add(secondService.Object);            

            var writer = new Mock<TextWriter>();
            var functions = new Functions();
            functions.Index(documentServices, indexService.Object, settings);
            indexService.Verify(x => x.DeleteIndex(It.IsAny<string>()), Times.Once());

            firstService.Verify(x => x.AddOrUpdateAll(), Times.Once());
            secondService.Verify(x => x.AddOrUpdateAll(), Times.Once());

            firstDisposableService.Verify(x => x.Dispose(), Times.Once());
            secondDisposableService.Verify(x => x.Dispose(), Times.Once());
            disposableIndexService.Verify(x => x.Dispose(), Times.Once());

        }

        [TestMethod]
        public void TestManualTrigger()
        {
            var indexService = new Mock<IIndexService>();
            var disposableIndexService = indexService.As<IDisposable>();

            var documentServices = new List<IDocumentService>();
            var firstService = new Mock<IDocumentService>();
            var firstDisposableService = firstService.As<IDisposable>();
            var secondDisposableService = firstService.As<IDisposable>();

            var secondService = new Mock<IDocumentService>();
            documentServices.Add(firstService.Object);
            documentServices.Add(secondService.Object);

            var writer = new Mock<TextWriter>();
            var functions = new Functions();
            functions.ManualTrigger(writer.Object, documentServices, indexService.Object, settings);
            indexService.Verify(x => x.DeleteIndex(It.IsAny<string>()), Times.Once());

            firstService.Verify(x => x.AddOrUpdateAll(), Times.Once());
            secondService.Verify(x => x.AddOrUpdateAll(), Times.Once());

            firstDisposableService.Verify(x => x.Dispose(), Times.Once());
            secondDisposableService.Verify(x => x.Dispose(), Times.Once());
            disposableIndexService.Verify(x => x.Dispose(), Times.Once());
        }
    }
}
