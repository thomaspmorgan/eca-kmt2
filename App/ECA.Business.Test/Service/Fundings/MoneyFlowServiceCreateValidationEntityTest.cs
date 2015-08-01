using ECA.Business.Service.Admin;
using ECA.Business.Service.Fundings;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ECA.Business.Test.Service.Fundings
{
    [TestClass]
    public class MoneyFlowServiceCreateValidationEntityTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;

            var instance = new MoneyFlowServiceCreateValidationEntity(description, value, sourceEntityTypeId, sourceEntityId, recipientEntityId, hasSourceEntityType, hasRecipientEntityType, transactionDate);
            Assert.AreEqual(value, instance.Value);
            Assert.AreEqual(description, instance.Description);
            Assert.AreEqual(transactionDate, instance.TransactionDate);
            Assert.AreEqual(hasSourceEntityType, instance.HasSourceEntityType);
            Assert.AreEqual(hasRecipientEntityType, instance.HasRecipientEntityType);
            Assert.AreEqual(sourceEntityId, instance.SourceEntityId);
            Assert.AreEqual(recipientEntityId, instance.RecipientEntityId);
            Assert.AreEqual(sourceEntityTypeId, instance.SourceEntityTypeId);
        }
    }
}
