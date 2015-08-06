using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.WebApi.Models.Fundings;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Fundings
{
    public class AdditionalMoneyFlowBindingModelTestClass : AdditionalMoneyFlowBindingModel<Project>
    {

        public int SourceEntityId { get; set; }


        public override int GetSourceTypeId()
        {
            return MoneyFlowSourceRecipientType.Project.Id;
        }

        public override int GetSourceEntityId()
        {
            return SourceEntityId;
        }
    }

    [TestClass]
    public class AdditionalMoneyFlowBindingModelTest
    {
        [TestMethod]
        public void TestToAdditionalMoneyFlow()
        {
            var model = new AdditionalMoneyFlowBindingModelTestClass
            {
                Description = "desc",
                FiscalYear = 1995,
                MoneyFlowStatusId = 1,
                RecipientEntityId = 2,
                RecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
                SourceEntityId = 3,
                TransactionDate = DateTimeOffset.UtcNow,
                Value = 1.00m,
            };
            var user = new User(1);
            var instance = model.ToAdditionalMoneyFlow(user);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
            Assert.AreEqual(model.Description, instance.Description);
            Assert.AreEqual(model.FiscalYear, instance.FiscalYear);
            Assert.AreEqual(model.MoneyFlowStatusId, instance.MoneyFlowStatusId);
            Assert.AreEqual(model.RecipientEntityId, instance.RecipientEntityId);
            Assert.AreEqual(model.RecipientTypeId, instance.RecipientEntityTypeId);
            Assert.AreEqual(model.SourceEntityId, instance.SourceEntityId);
            Assert.AreEqual(model.Value, instance.Value);
        }
    }
}
