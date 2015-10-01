using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Search;
using ECA.Business.Search;

namespace ECA.WebApi.Test.Controllers.Search
{
    [TestClass]
    public class ECADocumentViewModelTest
    {

        [TestMethod]
        public void TestConstructor_ECADocumentConstructor()
        {
            var documentTypeId = Guid.NewGuid();
            var id = 1;
            var key = new DocumentKey(documentTypeId, id);
            var document = new ECADocument();
            document.SetKey(key);

            var model = new ECADocumentViewModel(document);
            Assert.AreEqual(key, model.Key);
            Assert.AreEqual(document.Description, model.Description);
            Assert.AreEqual(document.DocumentTypeId, model.DocumentTypeId);
            Assert.AreEqual(document.DocumentTypeName, model.DocumentTypeName);
            Assert.AreEqual(document.Id, model.Id);
            Assert.AreEqual(document.Name, model.Name);
            Assert.AreEqual(document.OfficeSymbol, model.OfficeSymbol);

            Assert.IsTrue(Object.ReferenceEquals(document.Foci, model.Foci));
            Assert.IsTrue(Object.ReferenceEquals(document.Goals, model.Goals));
            Assert.IsTrue(Object.ReferenceEquals(document.Objectives, model.Objectives));
            Assert.IsTrue(Object.ReferenceEquals(document.PointsOfContact, model.PointsOfContact));
            Assert.IsTrue(Object.ReferenceEquals(document.Themes, model.Themes));
        }
    }
}
