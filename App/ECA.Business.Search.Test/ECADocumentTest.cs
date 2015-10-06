using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Azure.Search.Models;
using System.Collections.Generic;

namespace ECA.Business.Search.Test
{
    [TestClass]
    public class ECADocumentTest
    {

        [TestMethod]
        public void TestGetKey()
        {
            var key = new DocumentKey(Guid.NewGuid(), 1);
            var instance = new ECADocument();
            instance.SetKey(key);
            Assert.IsNotNull(instance.Id);
            Assert.AreEqual(key, instance.GetKey());
        }

        [TestMethod]
        public void TestSetKey_StringKey()
        {
            var key = new DocumentKey(Guid.NewGuid(), 1);
            var instance = new ECADocument();
            instance.SetKey(key.ToString());
            Assert.AreEqual(key, instance.GetKey());
            Assert.AreEqual(key.ToString(), instance.Id);
            Assert.AreEqual(key.DocumentTypeId.ToString(), instance.DocumentTypeId);
        }

        [TestMethod]
        public void TestSetKey_DocumentKey()
        {
            var key = new DocumentKey(Guid.NewGuid(), 1);
            var instance = new ECADocument();
            instance.SetKey(key);
            Assert.AreEqual(key, instance.GetKey());
            Assert.AreEqual(key.ToString(), instance.Id);
            Assert.AreEqual(key.DocumentTypeId.ToString(), instance.DocumentTypeId);
        }

        [TestMethod]
        public void TestSetKey_NullDocumentKey()
        {
            var key = new DocumentKey(Guid.NewGuid(), 1);
            var instance = new ECADocument();
            instance.SetKey(key.ToString());
            Assert.AreEqual(key, instance.GetKey());

            DocumentKey nullKey = null;
            instance.SetKey(nullKey);
            Assert.IsNull(instance.Id);
            Assert.IsNull(instance.DocumentTypeId);
        }

        [TestMethod]
        public void TestSetKey_NullStringValue()
        {
            var instance = new ECADocument();
            var key = new DocumentKey(Guid.NewGuid(), 1);
            instance.SetKey(key);
            Assert.IsNotNull(instance.GetKey());
            Assert.IsNotNull(instance.Id);

            string nullKey = null;
            instance.SetKey(nullKey);
            Assert.IsNull(instance.GetKey());
            Assert.IsNull(instance.Id);
        }

        [TestMethod]
        public void TestTypedConstructor_WithConfiguration()
        {
            var configuration = new TestDocumentConfiguration();
            var instance = new TestDocument();
            instance.Description = "desc";
            instance.Id = 1;
            instance.Name = "name";
            instance.OfficeSymbol = "office";
            instance.Status = "status";
            instance.Foci = new List<string> { "foci" };
            instance.Goals = new List<string> { "goals" };
            instance.Objectives = new List<string> { "objectives" };
            instance.Themes = new List<string> { "themes" };
            instance.PointsOfContact = new List<string> { "pocs" };
            instance.Websites = new List<string> { "web" };
            instance.Regions = new List<string> { "region" };
            instance.Countries = new List<string> { "country" };
            instance.Locations = new List<string> { "local" };

            var testDocumentProperties = typeof(TestDocument).GetProperties().OrderBy(x => x.Name).ToList();
            var ecaDocumentProperties = typeof(ECADocument).GetProperties().OrderBy(x => x.Name).ToList();
            //make sure all public properties are accounted for in TestDocument
            foreach(var testDocProperty in testDocumentProperties)
            {
                //check every property on TestDocument instance has a value.
                Assert.IsNotNull(testDocProperty.GetValue(instance), String.Format("TestDocument property [{0}] does not have a value.", testDocProperty.Name));
            }

            var document = new ECADocument<TestDocument>(configuration, instance);
            var documentKey = new DocumentKey(TestDocumentConfiguration.TEST_DOCUMENT_DOCUMENT_TYPE_ID, instance.Id);
            Assert.AreEqual(documentKey, document.GetKey());
            Assert.AreEqual(configuration.GetDocumentTypeId().ToString(), document.DocumentTypeId.ToString());
            Assert.AreEqual(configuration.GetDocumentTypeName(), document.DocumentTypeName);

            Assert.AreEqual(instance.Description, document.Description);
            Assert.AreEqual(instance.Name, document.Name);
            Assert.AreEqual(instance.OfficeSymbol, document.OfficeSymbol);
            Assert.AreEqual(instance.Status, document.Status);
            CollectionAssert.AreEqual(instance.Foci.ToList(), document.Foci.ToList());
            CollectionAssert.AreEqual(instance.Goals.ToList(), document.Goals.ToList());
            CollectionAssert.AreEqual(instance.Objectives.ToList(), document.Objectives.ToList());
            CollectionAssert.AreEqual(instance.Themes.ToList(), document.Themes.ToList());
            CollectionAssert.AreEqual(instance.PointsOfContact.ToList(), document.PointsOfContact.ToList());
            CollectionAssert.AreEqual(instance.Regions.ToList(), document.Regions.ToList());
            CollectionAssert.AreEqual(instance.Countries.ToList(), document.Countries.ToList());
            CollectionAssert.AreEqual(instance.Locations.ToList(), document.Locations.ToList());
            CollectionAssert.AreEqual(instance.Websites.ToList(), document.Websites.ToList());

            foreach (var ecaDocProperty in ecaDocumentProperties)
            {
                //make sure every eca document property has a value.
                Assert.IsNotNull(ecaDocProperty.GetValue(document));
            }
        }
    }
}
