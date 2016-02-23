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
    public class ParticipantPersonSevisControllerTest
    {
        private ParticipantPersonsSevisController controller;
        private Mock<IParticipantPersonsSevisService> serviceMock;
        private Mock<ISevisValidationService> validatorMock;
        private Mock<IUserProvider> userProvider;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IParticipantPersonsSevisService>();
            validatorMock = new Mock<ISevisValidationService>();
            userProvider = new Mock<IUserProvider>();
            controller = new ParticipantPersonsSevisController(serviceMock.Object, validatorMock.Object, userProvider.Object);
        }

        [TestMethod]
        public async Task TestPostSendToSevisAsync()
        {
            serviceMock.Setup(x => x.SendToSevisAsync(It.IsAny<int>(), It.IsAny<int[]>())).ReturnsAsync(new int[] { });
            var response = await controller.PostSendToSevisAsync(1, new int[] { 1, 2, 3 });
            serviceMock.Verify(x => x.SaveChangesAsync(), Times.Once);
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
