using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Data;
using ECA.Business.Service.Fundings;
using ECA.Business.Service;
using ECA.Core.Exceptions;

namespace ECA.Business.Test.Service.Fundings
{
    [TestClass]
    public class UpdatedMoneyFlowTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var updatorId = 1;
            var updator = new User(updatorId);
            var id = 2;
            var sourceEntityId = 3;
            var description = "description";
            var value = 1.0m;
            int moneyFlowStatusId = MoneyFlowStatus.Appropriated.Id;
            var transactionDate = DateTimeOffset.UtcNow;
            var entityTypeId = MoneyFlowSourceRecipientType.Accomodation.Id;
            var fiscalYear = 2000;
            var grantNumber = "grant";
            var isDirect = true;
            var instance = new UpdatedMoneyFlow(updator, id, sourceEntityId, entityTypeId, description, grantNumber, value, moneyFlowStatusId, transactionDate, fiscalYear, isDirect);
            Assert.AreEqual(id, instance.Id);
            Assert.AreEqual(description, instance.Description);
            Assert.AreEqual(fiscalYear, instance.FiscalYear);
            Assert.AreEqual(moneyFlowStatusId, instance.MoneyFlowStatusId);
            Assert.AreEqual(MoneyFlowType.Incoming.Id, instance.MoneyFlowTypeId);
            Assert.AreEqual(sourceEntityId, instance.SourceOrRecipientEntityId);
            Assert.AreEqual(transactionDate, instance.TransactionDate);
            Assert.AreEqual(value, instance.Value);
            Assert.AreEqual(entityTypeId, instance.SourceOrRecipientEntityTypeId);
            Assert.AreEqual(grantNumber, instance.GrantNumber);
            Assert.AreEqual(isDirect, instance.IsDirect);
        }

        [TestMethod]
        public void TestConstructor_UnknownMoneyFlowStatus()
        {
            var updatorId = 1;
            var updator = new User(updatorId);
            var id = 2;
            var sourceEntityId = 3;
            var description = "description";
            var value = 1.0m;
            int moneyFlowStatusId = -1;
            var transactionDate = DateTimeOffset.UtcNow;
            var fiscalYear = 2000;
            var grantNumber = "grant";
            var entityTypeId = MoneyFlowSourceRecipientType.Accomodation.Id;
            var isDirect = true;
            Action a = () => new UpdatedMoneyFlow(updator, id, sourceEntityId, entityTypeId, description, grantNumber, value, moneyFlowStatusId, transactionDate, fiscalYear, isDirect);
            a.ShouldThrow<UnknownStaticLookupException>().WithMessage(String.Format("The money flow status [{0}] is not supported.", moneyFlowStatusId));
        }
    }
}
