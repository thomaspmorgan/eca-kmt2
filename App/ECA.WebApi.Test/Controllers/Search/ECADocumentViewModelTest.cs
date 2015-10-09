using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Search;
using ECA.Business.Search;
using System.Collections.Generic;

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
            document.Description = "desc";
            document.Name = "name";
            document.OfficeSymbol = "office";
            document.Status = "status";
            document.Foci = new List<string> { "foci" };
            document.Goals = new List<string> { "goals" };
            document.Objectives = new List<string> { "objectives" };
            document.Themes = new List<string> { "themes" };
            document.PointsOfContact = new List<string> { "pocs" };
            document.Websites = new List<string> { "web" };
            document.Regions = new List<string> { "region" };
            document.Countries = new List<string> { "country" };
            document.Locations = new List<string> { "local" };
            document.DocumentTypeId = "type id";
            document.DocumentTypeName = "type name";
            document.StartDate = DateTimeOffset.Now.AddDays(-1.0);
            document.EndDate = DateTimeOffset.Now.AddDays(1.0);

            var model = new ECADocumentViewModel(document);
            Assert.AreEqual(key, model.Key);
            Assert.AreEqual(document.Description, model.Description);
            Assert.AreEqual(document.DocumentTypeId, model.DocumentTypeId);
            Assert.AreEqual(document.DocumentTypeName, model.DocumentTypeName);
            Assert.AreEqual(document.Id, model.Id);
            Assert.AreEqual(document.Name, model.Name);
            Assert.AreEqual(document.Status, model.Status);
            Assert.AreEqual(document.OfficeSymbol, model.OfficeSymbol);
            Assert.AreEqual(document.StartDate, model.StartDate);
            Assert.AreEqual(document.EndDate, model.EndDate);

            Assert.IsTrue(Object.ReferenceEquals(document.Foci, model.Foci));
            Assert.IsTrue(Object.ReferenceEquals(document.Goals, model.Goals));
            Assert.IsTrue(Object.ReferenceEquals(document.Objectives, model.Objectives));
            Assert.IsTrue(Object.ReferenceEquals(document.PointsOfContact, model.PointsOfContact));
            Assert.IsTrue(Object.ReferenceEquals(document.Themes, model.Themes));
            Assert.IsTrue(Object.ReferenceEquals(document.Regions, model.Regions));
            Assert.IsTrue(Object.ReferenceEquals(document.Countries, model.Countries));
            Assert.IsTrue(Object.ReferenceEquals(document.Locations, model.Locations));
            Assert.IsTrue(Object.ReferenceEquals(document.Websites, model.Websites));

            var properties = typeof(ECADocument).GetProperties();
            foreach (var property in properties)
            {
                //check every property on TestDocument instance has a value.
                Assert.IsNotNull(property.GetValue(document), String.Format("Property [{0}] on the document does not have a value.", property.Name));
                Assert.IsNotNull(property.GetValue(model), String.Format("Property [{0}] on the model does not have a value.", property.Name));
                Assert.AreEqual(property.GetValue(document), property.GetValue(model));
            }
        }
    }
}

