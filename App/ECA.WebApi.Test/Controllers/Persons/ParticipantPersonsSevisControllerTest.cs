using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Controllers.Persons;
using ECA.Business.Service.Persons;
using Moq;
using System.Web.Http.Results;
using System.Threading.Tasks;
using ECA.WebApi.Security;

namespace ECA.WebApi.Test.Controllers.Persons
{
    [TestClass]
    public class ParticipantPersonsSevisControllerTest
    {
        private ParticipantPersonsSevisController controller;
        private Mock<IParticipantPersonsSevisService> participantPersonSevisService;
        private Mock<IExchangeVisitorValidationService> validatorService;
        private Mock<IUserProvider> userProvider;

        [TestInitialize]
        public void TestInit()
        {
            participantPersonSevisService = new Mock<IParticipantPersonsSevisService>();
            validatorService = new Mock<IExchangeVisitorValidationService>();
            userProvider = new Mock<IUserProvider>();

            controller = new ParticipantPersonsSevisController(participantPersonSevisService.Object, validatorService.Object, userProvider.Object);
        }

        [TestMethod]
        public async Task TestPostSendToSevisAsync()
        {
            participantPersonSevisService.Setup(x => x.SendToSevisAsync(It.IsAny<int>(), It.IsAny<int[]>())).ReturnsAsync(new int[] { });
            var response = await controller.PostSendToSevisAsync(1, new int[] { 1, 2, 3 });
            participantPersonSevisService.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<int []>));
        }

        [TestMethod]
        public async Task TestPostSendToSevisAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostSendToSevisAsync(1, new int[] { 1, 2, 3 });
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
    }
}
