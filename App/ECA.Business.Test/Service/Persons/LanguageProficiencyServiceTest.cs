using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
using System.Threading.Tasks;
using ECA.Data;
using System.Linq;
using ECA.Business.Validation;
using ECA.Business.Validation.Model;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ECA.Business.Test.Service.LanguageProficiency
{
    [TestClass]
    public class LanguageProficiencyServiceTest
    {
        private TestEcaContext context;
        private LanguageProficiencyService languageProficiencyService;

        [TestInitialize]
        public void TestInit()
        {
            context = new TestEcaContext();
            languageProficiencyService = new LanguageProficiencyService(context);
        }

        [TestMethod]
        public async Task TestSendToSevis()
        {

            //Assert.AreEqual(SevisCommStatus.QueuedToSubmit.Id, newStatus.SevisCommStatusId);
        }

        

    }
}
