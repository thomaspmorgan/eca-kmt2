using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Fundings;
using ECA.Data;

namespace ECA.WebApi.Test.Models.Fundings
{
    [TestClass]
    public class AdditionalOrganizationMoneyFlowBindingModelTest
    {
        [TestMethod]
        public void TestGetSourceTypeId()
        {
            var instance = new AdditionalOrganizationMoneyFlowBindingModel();
            Assert.AreEqual(MoneyFlowSourceRecipientType.Organization.Id, instance.GetEntityTypeId());
        }

        [TestMethod]
        public void TestGetSourceEntityId()
        {
            var instance = new AdditionalOrganizationMoneyFlowBindingModel();
            instance.OrganizationId = 1;
            Assert.AreEqual(instance.OrganizationId, instance.GetEntityId());
        }
    }
}
