using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.WebApi.Models.Fundings;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Fundings
{
    public class AdditionalMoneyFlowBindingModelTestClass : AdditionalMoneyFlowBindingModel
    {

        public int SourceEntityId { get; set; }


        public override int GetEntityTypeId()
        {
            return MoneyFlowSourceRecipientType.Project.Id;
        }

        public override int GetEntityId()
        {
            return SourceEntityId;
        }
    }

    [TestClass]
    public class AdditionalMoneyFlowBindingModelTest
    {
        [TestMethod]
        public void TestToAdditionalMoneyFlow_IsOutgoing()
        {
            var model = new AdditionalMoneyFlowBindingModelTestClass
            {
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowStatusId = 1,
                ParentMoneyFlowId = 5,
                PeerEntityId = 2,
                PeerEntityTypeId = MoneyFlowSourceRecipientType.Project.Id,
                SourceEntityId = 3,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                IsOutgoing = true
            };
            var user = new User(1);
            var instance = model.ToAdditionalMoneyFlow(user);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
            Assert.AreEqual(model.Description, instance.Description);
            Assert.AreEqual(model.FiscalYear, instance.FiscalYear);
            Assert.AreEqual(model.MoneyFlowStatusId, instance.MoneyFlowStatusId);
            Assert.AreEqual(model.Value, instance.Value);

            Assert.AreEqual(model.ParentMoneyFlowId, instance.ParentMoneyFlowId);
            Assert.AreEqual(model.SourceEntityId, instance.SourceEntityId);
            Assert.AreEqual(model.GetEntityTypeId(), instance.SourceEntityTypeId);
            Assert.AreEqual(model.PeerEntityId, instance.RecipientEntityId);
            Assert.AreEqual(model.PeerEntityTypeId, instance.RecipientEntityTypeId);
        }

        [TestMethod]
        public void TestToAdditionalMoneyFlow_IsIncoming()
        {
            var model = new AdditionalMoneyFlowBindingModelTestClass
            {
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowStatusId = 1,
                PeerEntityId = 2,
                PeerEntityTypeId = MoneyFlowSourceRecipientType.Project.Id,
                SourceEntityId = 3,
                ParentMoneyFlowId = 5,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
                IsOutgoing = false
            };
            var user = new User(1);
            var instance = model.ToAdditionalMoneyFlow(user);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
            Assert.AreEqual(model.Description, instance.Description);
            Assert.AreEqual(model.FiscalYear, instance.FiscalYear);
            Assert.AreEqual(model.MoneyFlowStatusId, instance.MoneyFlowStatusId);
            Assert.AreEqual(model.Value, instance.Value);

            Assert.AreEqual(model.ParentMoneyFlowId, instance.ParentMoneyFlowId);
            Assert.AreEqual(model.PeerEntityId, instance.SourceEntityId);
            Assert.AreEqual(model.PeerEntityTypeId, instance.SourceEntityTypeId);
            Assert.AreEqual(model.SourceEntityId, instance.RecipientEntityId);
            Assert.AreEqual(model.GetEntityTypeId(), instance.RecipientEntityTypeId);
        }
    }
}
