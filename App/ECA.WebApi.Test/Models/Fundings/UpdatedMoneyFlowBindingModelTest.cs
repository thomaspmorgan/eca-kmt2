using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Fundings;
using ECA.Data;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Fundings
{
    [TestClass]
    public class UpdatedMoneyFlowBindingModelTest
    {
        [TestMethod]
        public void TestToUpdatedMoneyFlow()
        {
            var sourceEntityId = 2;
            var entityTypeId = MoneyFlowSourceRecipientType.Project.Id;
            var model = new UpdatedMoneyFlowBindingModel
            {
                Description = "desc",
                FiscalYear = 2015,
                Id = 1,
                MoneyFlowStatusId = MoneyFlowStatus.Budgeted.Id,
                TransactionDate = DateTimeOffset.UtcNow,
                Amount = 1.00m
            };
            var user = new User(1);
            var instance = model.ToUpdatedMoneyFlow(user, sourceEntityId, entityTypeId);
            Assert.IsTrue(Object.ReferenceEquals(user, instance.Audit.User));
            Assert.AreEqual(model.Description, instance.Description);
            Assert.AreEqual(model.FiscalYear, instance.FiscalYear);
            Assert.AreEqual(model.Id, instance.Id);
            Assert.AreEqual(model.MoneyFlowStatusId, instance.MoneyFlowStatusId);
            Assert.AreEqual(model.TransactionDate, instance.TransactionDate);
            Assert.AreEqual(model.Amount, instance.Value);
            Assert.AreEqual(sourceEntityId, instance.SourceOrRecipientEntityId);
            Assert.AreEqual(entityTypeId, instance.SourceOrRecipientEntityTypeId);
        }
    }
}
