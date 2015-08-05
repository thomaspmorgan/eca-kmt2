using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Models.Fundings;
using ECA.Data;

namespace ECA.WebApi.Test.Models.Fundings
{
    [TestClass]
    public class AdditionalProgramMoneyFlowBindingModelTest
    {
        [TestMethod]
        public void TestGetSourceTypeId()
        {
            var instance = new AdditionalProgramMoneyFlowBindingModel();
            Assert.AreEqual(MoneyFlowSourceRecipientType.Program.Id, instance.GetEntityTypeId());
        }

        [TestMethod]
        public void TestGetSourceEntityId()
        {
            var instance = new AdditionalProgramMoneyFlowBindingModel();
            instance.ProgramId = 1;
            Assert.AreEqual(instance.ProgramId, instance.GetEntityId());
        }
    }
}
