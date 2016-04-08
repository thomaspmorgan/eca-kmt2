using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Controllers.Persons;
using ECA.Business.Service.Persons;
using Moq;
using System.Web.Http.Results;
using System.Threading.Tasks;
using ECA.WebApi.Security;
using ECA.WebApi.Models.Person;
using System.Collections.Generic;
using ECA.Business.Service;
using System.Web;
using System.Web.Http;
using System.Net;

namespace ECA.WebApi.Test.Controllers.Persons
{
    [TestClass]
    public class ParticipantPersonsSevisControllerTest
    {
        private ParticipantPersonsSevisController controller;
        private Mock<IParticipantPersonsSevisService> participantPersonSevisService;
        private Mock<IUserProvider> userProvider;

        [TestInitialize]
        public void TestInit()
        {
            participantPersonSevisService = new Mock<IParticipantPersonsSevisService>();
            userProvider = new Mock<IUserProvider>();
            controller = new ParticipantPersonsSevisController(participantPersonSevisService.Object, userProvider.Object);
        }

        [TestMethod]
        public async Task TestPostSendToSevisAsync()
        {
            var model = new SendToSevisBindingModel
            {
                ParticipantIds = new List<int> { 1 },
                SevisOrgId = "org",
                SevisUsername = "user"
            };
            var projectId = 10;
            var user = new User(100);
            userProvider.Setup(x => x.HasSevisUserAccountAsync(It.IsAny<IWebApiUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);

            participantPersonSevisService.Setup(x => x.SendToSevisAsync(It.IsAny<ParticipantsToBeSentToSevis>())).ReturnsAsync(new int[] { });
            var response = await controller.PostSendToSevisAsync(1, projectId, model);
            participantPersonSevisService.Verify(x => x.SaveChangesAsync(), Times.Once);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<int[]>));
        }

        [TestMethod]
        public async Task TestPostSendToSevisAsync_UserDoesNotHaveProvidedSevisCredentials()
        {
            var model = new SendToSevisBindingModel
            {
                ParticipantIds = new List<int> { 1 },
                SevisOrgId = "org",
                SevisUsername = "user"
            };
            var projectId = 10;
            var user = new User(100);
            userProvider.Setup(x => x.HasSevisUserAccountAsync(It.IsAny<IWebApiUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);

            participantPersonSevisService.Setup(x => x.SendToSevisAsync(It.IsAny<ParticipantsToBeSentToSevis>())).ReturnsAsync(new int[] { });
            Func<Task> f =() => controller.PostSendToSevisAsync(1, projectId, model);
            f.ShouldThrow<HttpResponseException>().And.Response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [TestMethod]
        public async Task TestPostSendToSevisAsync_InvalidModel()
        {
            var model = new SendToSevisBindingModel
            {
                ParticipantIds = new List<int> { 1 },
                SevisOrgId = "org",
                SevisUsername = "user"
            };
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostSendToSevisAsync(1, 1, model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
    }
}
