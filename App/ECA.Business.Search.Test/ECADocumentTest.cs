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
        public void TestGoals()
        {
            var values = new string[] { "value1", "value2" };
            var instance = new ECADocument();
            instance.Goals = values;
            Assert.IsTrue(Object.ReferenceEquals(values, instance.Goals));
        }

        [TestMethod]
        public void TestThemes()
        {
            var values = new string[] { "value1", "value2" };
            var instance = new ECADocument();
            instance.Themes = values;
            Assert.IsTrue(Object.ReferenceEquals(values, instance.Themes));
        }

        [TestMethod]
        public void TestFoci()
        {
            var values = new string[] { "value1", "value2" };
            var instance = new ECADocument();
            instance.Foci = values;
            Assert.IsTrue(Object.ReferenceEquals(values, instance.Foci));
        }

        [TestMethod]
        public void TestPointsOfContact()
        {
            var values = new string[] { "value1", "value2" };
            var instance = new ECADocument();
            instance.PointsOfContact = values;
            Assert.IsTrue(Object.ReferenceEquals(values, instance.PointsOfContact));
        }

        [TestMethod]
        public void TestObjectives()
        {
            var values = new string[] { "value1", "value2" };
            var instance = new ECADocument();
            instance.Objectives = values;
            Assert.IsTrue(Object.ReferenceEquals(values, instance.Objectives));
        }

        [TestMethod]
        public void TestDescription()
        {
            var description = "desc";
            var instance = new ECADocument();
            instance.Description = description;
            Assert.AreEqual(description, instance.Description);
        }

        [TestMethod]
        public void TestId()
        {
            var value = "value";
            var instance = new ECADocument();
            instance.Id = value;
            Assert.AreEqual(value, instance.Id);
        }

        [TestMethod]
        public void TestName()
        {
            var value = "value";
            var instance = new ECADocument();
            instance.Name = value;
            Assert.AreEqual(value, instance.Name);
        }

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
        public void TestDocumentTypeId()
        {
            var key = new DocumentKey(Guid.NewGuid(), 1);
            var instance = new ECADocument();
            instance.SetKey(key);
            Assert.AreEqual(key.DocumentTypeId.ToString(), instance.DocumentTypeId);
            Assert.AreEqual(key.ToString(), instance.Id);
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
            instance.Foci = new List<string> { "foci" };
            instance.Goals = new List<string> { "goals" };
            instance.Objectives = new List<string> { "objectives" };
            instance.Themes = new List<string> { "themes" };
            instance.PointsOfContact = new List<string> { "pocs" };

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
            CollectionAssert.AreEqual(instance.Foci.ToList(), document.Foci.ToList());
            CollectionAssert.AreEqual(instance.Goals.ToList(), document.Goals.ToList());
            CollectionAssert.AreEqual(instance.Objectives.ToList(), document.Objectives.ToList());
            CollectionAssert.AreEqual(instance.Themes.ToList(), document.Themes.ToList());
            CollectionAssert.AreEqual(instance.PointsOfContact.ToList(), document.PointsOfContact.ToList());

            foreach (var ecaDocProperty in ecaDocumentProperties)
            {
                //make sure every eca document property has a value.
                Assert.IsNotNull(ecaDocProperty.GetValue(document));
            }
        }
    }
}
