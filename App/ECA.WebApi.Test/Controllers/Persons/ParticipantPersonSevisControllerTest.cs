using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Controllers.Persons;
using ECA.Business.Service.Persons;
using Moq;
using System.Web.Http.Results;
using System.Threading.Tasks;

namespace ECA.WebApi.Test.Controllers.Persons
{
    [TestClass]
    public class ParticipantPersonSevisControllerTest
    {
        private Mock<IParticipantPersonSevisService> serviceMock;
        private ParticipantPersonsSevisController controller;
        
        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IParticipantPersonSevisService>();
            controller = new ParticipantPersonsSevisController(serviceMock.Object);
        }

        [TestMethod]
        public async Task TestPostSendToSevisAsync()
        {
            serviceMock.Setup(x => x.SendToSevis(It.IsAny<int[]>())).ReturnsAsync(new int[] { });
            var response = await controller.PostSendToSevisAsync(new int[] { 1, 2, 3 });
            serviceMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<int []>));
        }

        [TestMethod]
        public async Task TestPostSendToSevisAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostSendToSevisAsync(new int[] { 1, 2, 3 });
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
    }
}
