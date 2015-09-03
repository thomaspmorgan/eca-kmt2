using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Fundings;
using ECA.Data;

namespace ECA.WebApi.Test.Models.Fundings
{
    [TestClass]
    public class AdditionalOfficeMoneyFlowBindingModelTest
    {
        [TestMethod]
        public void TestGetSourceTypeId()
        {
            var instance = new AdditionalOfficeMoneyFlowBindingModel();
            Assert.AreEqual(MoneyFlowSourceRecipientType.Office.Id, instance.GetEntityTypeId());
        }

        [TestMethod]
        public void TestGetSourceEntityId()
        {
            var instance = new AdditionalOfficeMoneyFlowBindingModel();
            instance.OfficeId = 1;
            Assert.AreEqual(instance.OfficeId, instance.GetEntityId());
        }
    }
}
