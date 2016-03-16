using ECA.Business.Service.Persons;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

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
