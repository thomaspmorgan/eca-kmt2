using ECA.Business.Service.Admin;
using ECA.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ECA.Business.Test.Service.Admin
{
    [TestClass]
    public class MoneyFlowServiceCreateValidationEntityTest
    {
        [TestMethod]
        public void TestConstructor()
        {
            var value = 1.00f;
            var description = "desc";
            var transactionDate = new DateTimeOffset();
            var entity = new MoneyFlowServiceCreateValidationEntity(
                value: value,
                description: description,
                transactionDate: transactionDate
                );
            Assert.AreEqual(value, entity.Value);
            Assert.AreEqual(description, entity.Description);
            Assert.AreEqual(transactionDate, entity.TransactionDate);
        }
    }
}
