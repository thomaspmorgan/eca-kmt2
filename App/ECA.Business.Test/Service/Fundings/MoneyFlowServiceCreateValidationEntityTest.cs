using ECA.Business.Service.Admin;
using System.Linq;
using ECA.Business.Service.Fundings;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

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
            var isParentMoneyFlowDirect = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            var fiscalYear = 2000;
            var allowedRecipientEntityTypeIds = new List<int> { 1, 2, 3, 3 };
            var allowedProjectParticipantIds = new List<int> { 5, 6, 7, 7 };
            var allowedSourceEntityTypeIds = new List<int> { 8, 8, 9, 9 };
            var parentMoneyFlowWithdrawalMaximum = 100m;
            var parentFiscalYear = 2100;

            var instance = new MoneyFlowServiceCreateValidationEntity(
                description, 
                value, 
                parentMoneyFlowWithdrawalMaximum,
                sourceEntityTypeId,
                recipientEntityTypeId,
                allowedRecipientEntityTypeIds,
                allowedSourceEntityTypeIds,
                allowedProjectParticipantIds,
                sourceEntityId, 
                recipientEntityId, 
                hasSourceEntityType, 
                hasRecipientEntityType, 
                transactionDate,
                fiscalYear,
                parentFiscalYear,
                isParentMoneyFlowDirect
                );
            Assert.AreEqual(value, instance.Value);
            Assert.AreEqual(parentMoneyFlowWithdrawalMaximum, instance.ParentMoneyFlowWithdrawlMaximum);
            Assert.AreEqual(description, instance.Description);
            Assert.AreEqual(transactionDate, instance.TransactionDate);
            Assert.AreEqual(hasSourceEntityType, instance.HasSourceEntityType);
            Assert.AreEqual(hasRecipientEntityType, instance.HasRecipientEntityType);
            Assert.AreEqual(sourceEntityId, instance.SourceEntityId);
            Assert.AreEqual(recipientEntityId, instance.RecipientEntityId);
            Assert.AreEqual(sourceEntityTypeId, instance.SourceEntityTypeId);
            Assert.AreEqual(recipientEntityTypeId, instance.RecipientEntityTypeId);
            Assert.AreEqual(fiscalYear, instance.FiscalYear);
            Assert.AreEqual(parentFiscalYear, instance.ParentFiscalYear);
            Assert.AreEqual(isParentMoneyFlowDirect, instance.IsParentMoneyFlowDirect);

            CollectionAssert.AreEqual(allowedRecipientEntityTypeIds.Distinct().ToList(), instance.AllowedRecipientEntityTypeIds.ToList());
            CollectionAssert.AreEqual(allowedSourceEntityTypeIds.Distinct().ToList(), instance.AllowedSourceEntityTypeIds.ToList());
            CollectionAssert.AreEqual(allowedProjectParticipantIds.Distinct().ToList(), instance.AllowedProjectParticipantIds.ToList());
        }

        [TestMethod]
        public void TestConstructor_NullIdsLists()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            var isParentMoneyFlowDirect = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            var fiscalYear = 2000;
            var parentFiscalYear = 2100;
            var parentMoneyFlowWithdrawalMaximum = 100m;
            List<int> allowedRecipientEntityTypeIds = null;
            List<int> allowedProjectParticipantIds = null;
            List<int> allowedSourceEntityTypeIds = null;


            var instance = new MoneyFlowServiceCreateValidationEntity(
                description,
                value,
                parentMoneyFlowWithdrawalMaximum,
                sourceEntityTypeId,
                recipientEntityTypeId,
                allowedRecipientEntityTypeIds,
                allowedSourceEntityTypeIds,
                allowedProjectParticipantIds,
                sourceEntityId,
                recipientEntityId,
                hasSourceEntityType,
                hasRecipientEntityType,
                transactionDate,
                fiscalYear,
                parentFiscalYear,
                isParentMoneyFlowDirect);
            Assert.IsNotNull(instance.AllowedRecipientEntityTypeIds);
            Assert.IsNotNull(instance.AllowedProjectParticipantIds);
            Assert.IsNotNull(instance.AllowedSourceEntityTypeIds);

        }
    }
}
