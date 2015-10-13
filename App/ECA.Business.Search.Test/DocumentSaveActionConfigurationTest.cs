using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ECA.Business.Search.Test
{
    [TestClass]
    public class DocumentSaveActionConfigurationTest
    {
        [TestMethod]
        public void TestConstructor_ExpressionAndGuidConstructor()
        {
            var instance = new DocumentSaveActionConfiguration<SimpleEntity>(x => x.Id, SimpleEntityConfiguration.SIMPLE_ENTITY_DOCUMENT_TYPE_ID);
            Assert.IsNotNull(instance.CreatedExclusionRules);
            Assert.IsNotNull(instance.ModifiedExclusionRules);
            Assert.IsNotNull(instance.DeletedExclusionRules);
            Assert.IsNotNull(instance.IdDelegate);
            Assert.IsNotNull(instance.DocumentTypeIdDelegate);

            var entity = new SimpleEntity();
            entity.Id = 10;

            Assert.AreEqual(entity.Id, instance.GetId(entity));
            Assert.AreEqual(SimpleEntityConfiguration.SIMPLE_ENTITY_DOCUMENT_TYPE_ID, instance.GetDocumentTypeId(entity));
        }        

        [TestMethod]
        public void TestConstructor_IdExpressionAndGuidExpressionConstructor()
        {
            var documentTypeId = Guid.NewGuid();
            var instance = new DocumentSaveActionConfiguration<SimpleEntity>(x => x.Id, (a) => documentTypeId);
            Assert.IsNotNull(instance.CreatedExclusionRules);
            Assert.IsNotNull(instance.ModifiedExclusionRules);
            Assert.IsNotNull(instance.DeletedExclusionRules);
            Assert.IsNotNull(instance.IdDelegate);
            Assert.IsNotNull(instance.DocumentTypeIdDelegate);

            var entity = new SimpleEntity();
            entity.Id = 10;

            Assert.AreEqual(entity.Id, instance.GetId(entity));
            Assert.AreEqual(documentTypeId, instance.GetDocumentTypeId(entity));
        }
    }
}
