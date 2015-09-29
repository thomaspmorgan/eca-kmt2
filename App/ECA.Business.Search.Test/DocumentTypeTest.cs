using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Linq;

namespace ECA.Business.Search.Test
{
    [TestClass]
    public class DocumentTypeTest
    {
        [TestMethod]
        public void TestAllStaticPropertiesUnique()
        {
            var t = typeof(DocumentType);
            var staticProperties = t.GetProperties(BindingFlags.Static | BindingFlags.Public);
            var allComparisonTypes = staticProperties.Select(x => x.GetValue(null) as DocumentType).ToList();
            foreach (var prop in staticProperties)
            {
                var testValue = (DocumentType)prop.GetValue(null);
                var allValues = allComparisonTypes.Where(x => x.Name == testValue.Name).ToList();
                var allHashCodes = allComparisonTypes.Where(x => x.GetHashCode() == testValue.GetHashCode()).ToList();

                //check all string values are unique...
                Assert.AreEqual(1, allValues.Count);

                //check all hash codes are unique...
                Assert.AreEqual(1, allHashCodes.Count);

                //check overridden equals are correct...
                var allOtherValues = allComparisonTypes.Where(x => x.Name != testValue.Name).ToList();
                allOtherValues.ForEach(x =>
                {
                    Assert.IsFalse(x == testValue);
                    Assert.IsTrue(x != testValue);

                    Assert.IsFalse(x.Equals(testValue));
                });
            }

        }

        [TestMethod]
        public void TestGetHashCode()
        {
            var c = DocumentType.Program;
            Assert.AreEqual(c.Id.GetHashCode(), c.GetHashCode());
        }

        [TestMethod]
        public void TestEquals()
        {
            var c = DocumentType.Program;
            var otherC = DocumentType.Program;
            Assert.IsTrue(c.Equals(otherC));
        }

        [TestMethod]
        public void TestEquals_NullTestObject()
        {
            var c = DocumentType.Program;
            Assert.IsFalse(c.Equals(null));
        }

        [TestMethod]
        public void TestEquals_DifferentTypeObject()
        {
            var c = DocumentType.Program;
            Assert.IsFalse(c.Equals(1));
        }

        [TestMethod]
        public void TestEqualOperator()
        {
            var c = DocumentType.Program;
            var otherC = DocumentType.Program;
            Assert.IsTrue(c == otherC);
        }

        [TestMethod]
        public void TestNotEqualOperator()
        {
            var c = DocumentType.Program;
            var otherC = DocumentType.Program;
            Assert.IsFalse(c != otherC);
        }

        [TestMethod]
        public void TestToString()
        {
            Assert.IsNotNull(DocumentType.Program.ToString());
        }

        [TestMethod]
        public void TestToDocumentType()
        {
            Assert.AreEqual(DocumentType.Program, DocumentType.ToDocumentType(DocumentType.Program.Id));
            Assert.AreEqual(DocumentType.Project, DocumentType.ToDocumentType(DocumentType.Project.Id));
        }
    }
}
