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
        public void TestGetIndex()
        {
            var testDocument = new TestDocument();
            testDocument.Id = 1;
            testDocument.Description = "desc";
            testDocument.Name = "name";
            testDocument.Subtitle = "subtitle";

            var documentType = testDocument.GetDocumentType();
            var additionalFields = testDocument.GetDocumentFields();
            var ecaDocument = new SimpleDocument(testDocument);
            var index = ecaDocument.GetIndex();
            Assert.AreEqual(testDocument.GetDocumentType().IndexName, index.Name);

            var idField = index.Fields.Where(x => x.Name == ECADocument.ID_KEY).FirstOrDefault();
            Assert.IsNotNull(idField);
            Assert.IsTrue(idField.IsKey);
            Assert.IsTrue(idField.IsRetrievable);

            var titleField = index.Fields.Where(x => x.Name == ECADocument.TITLE_KEY).FirstOrDefault();
            Assert.IsNotNull(titleField);
            Assert.IsTrue(titleField.IsRetrievable);

            var descriptionField = index.Fields.Where(x => x.Name == ECADocument.DESCRIPTION_KEY).FirstOrDefault();
            Assert.IsNotNull(descriptionField);
            Assert.IsTrue(descriptionField.IsRetrievable);

            var subtitleField = index.Fields.Where(x => x.Name == ECADocument.SUBTITLE_KEY).FirstOrDefault();
            Assert.IsNotNull(subtitleField);
            Assert.IsTrue(subtitleField.IsRetrievable);

            var documentTypeField = index.Fields.Where(x => x.Name == ECADocument.DOCUMENT_TYPE_ID_KEY).FirstOrDefault();
            Assert.IsNotNull(documentTypeField);
            Assert.IsTrue(documentTypeField.IsFacetable);
            Assert.IsTrue(documentTypeField.IsRetrievable);

            foreach (var field in testDocument.GetDocumentFields())
            {
                var additionalField = index.Fields.Where(x => x.Name == field).FirstOrDefault();
                Assert.IsNotNull(additionalField);
                Assert.AreEqual(DataType.String, additionalField.Type);
                Assert.IsTrue(additionalField.IsSearchable);
                Assert.IsTrue(additionalField.IsRetrievable);
            }

        }
    }
}
