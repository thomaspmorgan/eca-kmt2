using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Azure.Search.Models;

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
        public void TestSetKey_DocumentKey()
        {
            var key = new DocumentKey(DocumentType.Program, 1);
            var instance = new ECADocument();
            instance.SetKey(key);
            Assert.AreEqual(key, instance.GetKey());
            Assert.AreEqual(key.ToString(), instance.Id);
        }

        [TestMethod]
        public void TestSetKey_StringKey()
        {
            var key = new DocumentKey(DocumentType.Program, 1);
            var instance = new ECADocument();
            instance.SetKey(key.ToString());
            Assert.AreEqual(key, instance.GetKey());
            Assert.AreEqual(key.ToString(), instance.Id);
        }

        [TestMethod]
        public void TestKey_NullDocumentKeyValue()
        {
            var instance = new ECADocument();
            var key = new DocumentKey(DocumentType.Program, 1);
            instance.SetKey(key);
            Assert.IsNotNull(instance.GetKey());
            Assert.IsNotNull(instance.Id);

            DocumentKey nullKey = null;
            instance.SetKey(nullKey);
            Assert.IsNull(instance.GetKey());
            Assert.IsNull(instance.Id);
        }

        [TestMethod]
        public void TestKey_NullStringValue()
        {
            var instance = new ECADocument();
            var key = new DocumentKey(DocumentType.Program, 1);
            instance.SetKey(key);
            Assert.IsNotNull(instance.GetKey());
            Assert.IsNotNull(instance.Id);

            string nullKey = null;
            instance.SetKey(nullKey);
            Assert.IsNull(instance.GetKey());
            Assert.IsNull(instance.Id);
        }

        [TestMethod]
        public void TestDocumentType()
        {
            var key = new DocumentKey(DocumentType.Program, 1);
            var instance = new ECADocument();
            instance.SetKey(key);
            Assert.AreEqual(key.DocumentType, instance.GetDocumentType());
        }

        [TestMethod]
        public void TestDocumentType_NullKey()
        {
            DocumentKey nullKey = null;
            var instance = new ECADocument();
            instance.SetKey(nullKey);
            Assert.IsNull(instance.GetDocumentType());
        }

        [TestMethod]
        public void TestTypedConstructor_WithConfiguration()
        {
            var configuration = new TestDocumentConfiguration();
            var instance = new TestDocument();
            instance.Description = "desc";
            instance.Id = 1;
            instance.Name = "name";

            var document = new ECADocument<TestDocument>(configuration, instance);
            Assert.AreEqual(new DocumentKey(configuration.GetDocumentType(), instance.Id), document.GetKey());
            Assert.AreEqual(configuration.GetDocumentType(), document.GetKey().DocumentType);
            Assert.AreEqual(instance.Description, document.Description);
            Assert.AreEqual(instance.Name, document.Name);
        }
    }
}
