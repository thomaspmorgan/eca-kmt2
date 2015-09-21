﻿using ECA.Business.Service.Admin;
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
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            var fiscalYear = 2000;
            var list = new List<int> { 1, 2, 3, 3 };

            var instance = new MoneyFlowServiceCreateValidationEntity(
                description, 
                value, 
                sourceEntityTypeId,
                recipientEntityTypeId,
                list,
                sourceEntityId, 
                recipientEntityId, 
                hasSourceEntityType, 
                hasRecipientEntityType, 
                transactionDate,
                fiscalYear);
            Assert.AreEqual(value, instance.Value);
            Assert.AreEqual(description, instance.Description);
            Assert.AreEqual(transactionDate, instance.TransactionDate);
            Assert.AreEqual(hasSourceEntityType, instance.HasSourceEntityType);
            Assert.AreEqual(hasRecipientEntityType, instance.HasRecipientEntityType);
            Assert.AreEqual(sourceEntityId, instance.SourceEntityId);
            Assert.AreEqual(recipientEntityId, instance.RecipientEntityId);
            Assert.AreEqual(sourceEntityTypeId, instance.SourceEntityTypeId);
            Assert.AreEqual(recipientEntityTypeId, instance.RecipientEntityTypeId);
            Assert.AreEqual(fiscalYear, instance.FiscalYear);

            CollectionAssert.AreEqual(list.Distinct().ToList(), instance.AllowedRecipientEntityTypeIds.ToList());
        }

        [TestMethod]
        public void TestConstructor_NullAllowedRecipientEntityTypeIds()
        {
            var value = 1.00m;
            var description = "description";
            var transactionDate = DateTimeOffset.UtcNow;
            var hasSourceEntityType = true;
            var hasRecipientEntityType = true;
            int? sourceEntityId = 1;
            int? recipientEntityId = 2;
            var sourceEntityTypeId = MoneyFlowSourceRecipientType.Post.Id;
            var recipientEntityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            var fiscalYear = 2000;
            List<int> list = null;


            var instance = new MoneyFlowServiceCreateValidationEntity(
                description,
                value,
                sourceEntityTypeId,
                recipientEntityTypeId,
                list,
                sourceEntityId,
                recipientEntityId,
                hasSourceEntityType,
                hasRecipientEntityType,
                transactionDate,
                fiscalYear);
            Assert.IsNotNull(instance.AllowedRecipientEntityTypeIds);
           
        }
    }
}
