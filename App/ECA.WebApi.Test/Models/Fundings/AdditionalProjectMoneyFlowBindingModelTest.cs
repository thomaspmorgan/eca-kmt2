using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Fundings;
using ECA.Data;

namespace ECA.WebApi.Test.Models.Fundings
{
    [TestClass]
    public class AdditionalProjectMoneyFlowBindingModelTest
    {
        [TestMethod]
        public void TestGetSourceTypeId()
        {
            var instance = new AdditionalProjectMoneyFlowBindingModel();
            Assert.AreEqual(MoneyFlowSourceRecipientType.Project.Id, instance.GetEntityTypeId());
        }

        [TestMethod]
        public void TestGetSourceEntityId()
        {
            var instance = new AdditionalProjectMoneyFlowBindingModel();
            instance.ProjectId = 1;
            Assert.AreEqual(instance.ProjectId, instance.GetEntityId());
        }
    }
}
