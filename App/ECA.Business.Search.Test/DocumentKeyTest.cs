using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Search.Test
{
    [TestClass]
    public class DocumentKeyTest
    {
        [TestMethod]
        public void TestConstructor_StringKey()
        {
            var documentTypeId = Guid.NewGuid();
            var id = 123;

            var key = new DocumentKey(documentTypeId, id);
            var keyString = key.ToString();

            var testKey = new DocumentKey(keyString);
            Assert.AreEqual(id, testKey.Value);
            Assert.AreEqual(documentTypeId, testKey.DocumentTypeId);
            Assert.AreEqual(DocumentKeyType.Int, testKey.KeyType);
        }

        [TestMethod]
        public void TestConstructor_UnknownDocumentKeyType()
        {
            var documentTypeId = Guid.NewGuid();
            var id = 'c';

            Action a = () => new DocumentKey(documentTypeId, id);
            a.ShouldThrow<NotSupportedException>().WithMessage("The id type is not supported.");
        }

        [TestMethod]
        public void TestConstructor_DocumentTypeAndObjectKey()
        {
            var documentTypeId = Guid.NewGuid();
            var id = 123;

            var documentKey = new DocumentKey(documentTypeId, id);
            Assert.AreEqual(id, documentKey.Value);
            Assert.AreEqual(documentTypeId, documentKey.DocumentTypeId);
            Assert.AreEqual(DocumentKeyType.Int, documentKey.KeyType);
        }

        [TestMethod]
        public void TestGetDocumentKeyType_IntKey()
        {
            var id = 123;

            var testKeyType = DocumentKey.GetDocumentKeyType(id);
            Assert.AreEqual(DocumentKeyType.Int, testKeyType);
        }

        [TestMethod]
        public void TestGetDocumentKeyType_StringKey()
        {
            var id = "abc";

            var testKeyType = DocumentKey.GetDocumentKeyType(id);
            Assert.AreEqual(DocumentKeyType.String, testKeyType);
        }

        [TestMethod]
        public void TestGetDocumentKeyType_GuidKey()
        {
            var id = Guid.NewGuid();

            var testKeyType = DocumentKey.GetDocumentKeyType(id);
            Assert.AreEqual(DocumentKeyType.Guid, testKeyType);
        }
        [TestMethod]
        public void TestGetDocumentKeyType_LongKey()
        {
            var id = 123L;

            var testKeyType = DocumentKey.GetDocumentKeyType(id);
            Assert.AreEqual(DocumentKeyType.Long, testKeyType);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestGetDocumentKeyType_UnknownType()
        {
            var testKeyType = DocumentKey.GetDocumentKeyType('c');
        }


        [TestMethod]
        public void ParseId_IntId()
        {
            var documentTypeId = Guid.NewGuid();
            var documentKey = new DocumentKey(documentTypeId, 123);

            var keyType = DocumentKeyType.Int;
            var id = 123;
            var idAsString = id.ToString();
            var parsedId = documentKey.ParseId(keyType, idAsString);
            Assert.AreEqual(id, parsedId);
        }

        [TestMethod]
        public void ParseId_GuidId()
        {
            var documentTypeId = Guid.NewGuid();
            var documentKey = new DocumentKey(documentTypeId, 123);

            var keyType = DocumentKeyType.Guid;
            var id = Guid.NewGuid();
            var idAsString = id.ToString();
            var parsedId = documentKey.ParseId(keyType, idAsString);
            Assert.AreEqual(id, parsedId);
        }

        [TestMethod]
        public void ParseId_StringId()
        {
            var documentTypeId = Guid.NewGuid();
            var documentKey = new DocumentKey(documentTypeId, 123);

            var keyType = DocumentKeyType.String;
            var id = "abc";
            var idAsString = id.ToString();
            var parsedId = documentKey.ParseId(keyType, idAsString);
            Assert.AreEqual(id, parsedId);
        }

        [TestMethod]
        public void ParseId_LongId()
        {
            var documentTypeId = Guid.NewGuid();
            var documentKey = new DocumentKey(documentTypeId, 123);

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
            var documentTypeId = Guid.NewGuid();
            var documentKey = new DocumentKey(documentTypeId, 123);

            var keyType = (DocumentKeyType)Enum.ToObject(typeof(DocumentKeyType), -1);
            var id = 123L;
            var idAsString = id.ToString();
            var parsedId = documentKey.ParseId(keyType, idAsString);
        }

        [TestMethod]
        public void TestEquals_OtherValueIsNull()
        {
            var documentTypeId = Guid.NewGuid();
            var documentKey = new DocumentKey(documentTypeId, 1);
            Assert.IsFalse(documentKey.Equals(null));
        }

        [TestMethod]
        public void TestEquals_DifferentObjectType()
        {
            var documentTypeId = Guid.NewGuid();
            var documentKey = new DocumentKey(documentTypeId, 1);
            Assert.IsFalse(documentKey.Equals(1));
        }

        [TestMethod]
        public void TestEquals_DifferentDocumentType()
        {
            var documentTypeId = Guid.NewGuid();
            var id = 1;
            var otherDocTypeId = Guid.NewGuid();
            var otherId = 1;
            var documentKey = new DocumentKey(documentTypeId, id);
            var otherDocumentKey = new DocumentKey(otherDocTypeId, otherId);
            Assert.IsFalse(documentKey.Equals(otherDocumentKey));
        }

        [TestMethod]
        public void TestEquals_DifferentId()
        {
            var docTypeId = Guid.NewGuid();
            var id = 1;
            var otherDocType = docTypeId;
            var otherId = 2;
            var documentKey = new DocumentKey(docTypeId, id);
            var otherDocumentKey = new DocumentKey(otherDocType, otherId);
            Assert.IsFalse(documentKey.Equals(otherDocumentKey));
        }

        [TestMethod]
        public void TestEquals_SameIdSameDocType()
        {
            var docTypeId = Guid.NewGuid();
            var id = 1;
            var otherDocType = docTypeId;
            var otherId = 1;
            var documentKey = new DocumentKey(docTypeId, id);
            var otherDocumentKey = new DocumentKey(otherDocType, otherId);
            Assert.IsTrue(documentKey.Equals(otherDocumentKey));
        }

        [TestMethod]
        public void TestEqualOperator_SameObjectReference()
        {
            var docTypeId = Guid.NewGuid();
            var id = 1;
            var documentKey = new DocumentKey(docTypeId, id);
            var otherDocumentKey = new DocumentKey(docTypeId, id);
            Assert.IsTrue(documentKey == otherDocumentKey);
        }

        [TestMethod]
        public void TestEqualOperator_LeftSideNull()
        {
            var docTypeId = Guid.NewGuid();
            var id = 1;
            var documentKey = new DocumentKey(docTypeId, id);
            Assert.IsFalse(null == documentKey);
        }

        [TestMethod]
        public void TestEqualOperator_RightSideNull()
        {
            var docTypeId = Guid.NewGuid();
            var id = 1;
            var documentKey = new DocumentKey(docTypeId, id);
            Assert.IsFalse(documentKey == null);
        }

        [TestMethod]
        public void TestEqualOperator_AreEqual()
        {
            var docTypeId = Guid.NewGuid();
            var id = 1;
            var otherDocTypeId = docTypeId;
            var otherId = 1;
            var documentKey = new DocumentKey(docTypeId, id);
            var otherDocumentKey = new DocumentKey(otherDocTypeId, otherId);
            Assert.IsTrue(documentKey == otherDocumentKey);
        }

        [TestMethod]
        public void TestEqualOperator_AreNotEqual()
        {
            var docTypeId = Guid.NewGuid();
            var id = 1;
            var otherDocTypeId = Guid.NewGuid();
            var otherId = 2;
            var documentKey = new DocumentKey(docTypeId, id);
            var otherDocumentKey = new DocumentKey(otherDocTypeId, otherId);
            Assert.IsFalse(documentKey == otherDocumentKey);
        }
        [TestMethod]
        public void TestNotEqualOperator_AreEqual()
        {
            var docTypeId = Guid.NewGuid();
            var id = 1;
            var otherDocType = docTypeId;
            var otherId = 1;
            var documentKey = new DocumentKey(docTypeId, id);
            var otherDocumentKey = new DocumentKey(otherDocType, otherId);
            Assert.IsFalse(documentKey != otherDocumentKey);
        }

        [TestMethod]
        public void TestNotEqualOperator_AreNotEqual()
        {
            var docTypeId = Guid.NewGuid();
            var id = 1;
            var otherTypeId = Guid.NewGuid();
            var otherId = 2;
            var documentKey = new DocumentKey(docTypeId, id);
            var otherDocumentKey = new DocumentKey(otherTypeId, otherId);
            Assert.IsTrue(documentKey != otherDocumentKey);
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            var docTypeId = Guid.NewGuid();
            var id = 2;
            var documentKey = new DocumentKey(docTypeId, id);
            Assert.IsNotNull(documentKey.GetHashCode());
        }

        [TestMethod]
        public void TestTryParse_IsValidDocumentKey()
        {
            var documentKey = new DocumentKey(Guid.NewGuid(), 1);
            var documentKeyAsString = documentKey.ToString();

            DocumentKey testKey;
            var result = DocumentKey.TryParse(documentKeyAsString, out testKey);
            Assert.IsTrue(result);
            Assert.IsNotNull(testKey);
            Assert.AreEqual(documentKey, testKey);
        }

        [TestMethod]
        public void TestTryParse_IsNotValidDocumentKey()
        {
            var documentKey = new DocumentKey(Guid.NewGuid(), 1);
            var documentKeyAsString = documentKey.ToString();

            DocumentKey testKey = null;
            var result = DocumentKey.TryParse("abc", out testKey);
            Assert.IsFalse(result);
            Assert.IsNull(testKey);
        }
    }
}
