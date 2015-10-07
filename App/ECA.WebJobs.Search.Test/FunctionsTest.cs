using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ECA.Business.Search;
using Moq;
using System.IO;

namespace ECA.WebJobs.Search.Test
{
    [TestClass]
    public class FunctionsTest
    {
        [TestMethod]
        public void TestIndex()
        {
            var documentServices = new List<IDocumentService>();
            var firstService = new Mock<IDocumentService>();
            var firstDisposableService = firstService.As<IDisposable>();

            var secondService = new Mock<IDocumentService>();
            documentServices.Add(firstService.Object);
            documentServices.Add(secondService.Object);

            var writer = new Mock<TextWriter>();
            var functions = new Functions();
            functions.Index(writer.Object, documentServices);
            firstService.Verify(x => x.AddOrUpdateAll(), Times.Once());
            secondService.Verify(x => x.AddOrUpdateAll(), Times.Once());

            firstDisposableService.Verify(x => x.Dispose(), Times.Once());
        }

        [TestMethod]
        public void TestManualTrigger()
        {
            var documentServices = new List<IDocumentService>();
            var firstService = new Mock<IDocumentService>();
            var firstDisposableService = firstService.As<IDisposable>();

            var secondService = new Mock<IDocumentService>();
            documentServices.Add(firstService.Object);
            documentServices.Add(secondService.Object);

            var writer = new Mock<TextWriter>();
            var functions = new Functions();
            functions.ManualTrigger(writer.Object, documentServices);
            firstService.Verify(x => x.AddOrUpdateAll(), Times.Once());
        }
    }
}
