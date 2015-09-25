using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Search.Test
{
    [TestClass]
    public class DocumentKeyTest
    {
        [TestMethod]
        public void TestConstructor_StringKey()
        {
            var documentType = DocumentType.Program;
            var id = 123;

            var key = new DocumentKey(documentType, id);
            var keyString = key.ToString();

            var testKey = new DocumentKey(keyString);
            Assert.AreEqual(id, testKey.Value);
            Assert.AreEqual(documentType, testKey.DocumentType);
            Assert.AreEqual(DocumentKeyType.Int, testKey.KeyType);
        }

        [TestMethod]
        public void TestConstructor_DocumentTypeAndObjectKey()
        {
            var documentType = DocumentType.Program;
            var id = 123;

            var documentKey = new DocumentKey(documentType, id);
            Assert.AreEqual(id, documentKey.Value);
            Assert.AreEqual(documentType, documentKey.DocumentType);
            Assert.AreEqual(DocumentKeyType.Int, documentKey.KeyType);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestConstructor_UnknownDocumentKeyType()
        {
            var keyAsString = "1-1-100";

            var documentKey = new DocumentKey(keyAsString);
        }

        [TestMethod]
        public void TestGetDocumentKeyType_IntKey()
        {
            var documentType = DocumentType.Program;
            var id = 123;

            var documentKey = new DocumentKey(documentType, id);
            var testKeyType = documentKey.GetDocumentKeyType(id);
            Assert.AreEqual(DocumentKeyType.Int, testKeyType);
        }

        [TestMethod]
        public void TestGetDocumentKeyType_StringKey()
        {
            var documentType = DocumentType.Program;
            var id = "abc";

            var documentKey = new DocumentKey(documentType, id);
            var testKeyType = documentKey.GetDocumentKeyType(id);
            Assert.AreEqual(DocumentKeyType.String, testKeyType);
        }

        [TestMethod]
        public void TestGetDocumentKeyType_GuidKey()
        {
            var documentType = DocumentType.Program;
            var id = Guid.NewGuid();

            var documentKey = new DocumentKey(documentType, id);
            var testKeyType = documentKey.GetDocumentKeyType(id);
            Assert.AreEqual(DocumentKeyType.Guid, testKeyType);
        }
        [TestMethod]
        public void TestGetDocumentKeyType_LongKey()
        {
            var documentType = DocumentType.Program;
            var id = 123L;

            var documentKey = new DocumentKey(documentType, id);
            var testKeyType = documentKey.GetDocumentKeyType(id);
            Assert.AreEqual(DocumentKeyType.Long, testKeyType);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestGetDocumentKeyType_UnknownType()
        {
            var documentType = DocumentType.Program;
            var id = 123;

            var documentKey = new DocumentKey(documentType, id);
            var testKeyType = documentKey.GetDocumentKeyType('c');
        }


        [TestMethod]
        public void ParseId_IntId()
        {
            var documentKey = new DocumentKey(DocumentType.Program, 123);

            var keyType = DocumentKeyType.Int;
            var id = 123;
            var idAsString = id.ToString();
            var parsedId = documentKey.ParseId(keyType, idAsString);
            Assert.AreEqual(id, parsedId);
        }

        [TestMethod]
        public void ParseId_GuidId()
        {
            var documentKey = new DocumentKey(DocumentType.Program, 123);

            var keyType = DocumentKeyType.Guid;
            var id = Guid.NewGuid();
            var idAsString = id.ToString();
            var parsedId = documentKey.ParseId(keyType, idAsString);
            Assert.AreEqual(id, parsedId);
        }

        [TestMethod]
        public void ParseId_StringId()
        {
            var documentKey = new DocumentKey(DocumentType.Program, 123);

            var keyType = DocumentKeyType.String;
            var id = "abc";
            var idAsString = id.ToString();
            var parsedId = documentKey.ParseId(keyType, idAsString);
            Assert.AreEqual(id, parsedId);
        }

        [TestMethod]
        public void ParseId_LongId()
        {
            var documentKey = new DocumentKey(DocumentType.Program, 123);

            var keyType = DocumentKeyType.Long;
            var id = 123L;
            var idAsString = id.ToString();
            var parsedId = documentKey.ParseId(keyType, idAsString);
            Assert.AreEqual(id, parsedId);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ParseId_UnsupportedKeyType()
        {
            var documentKey = new DocumentKey(DocumentType.Program, 123);

            var keyType = (DocumentKeyType)Enum.ToObject(typeof(DocumentKeyType), -1);
            var id = 123L;
            var idAsString = id.ToString();
            var parsedId = documentKey.ParseId(keyType, idAsString);
        }
    }
}
