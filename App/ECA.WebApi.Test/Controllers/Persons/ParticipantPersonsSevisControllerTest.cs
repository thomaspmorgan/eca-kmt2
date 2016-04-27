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
using ECA.WebApi.Models.Query;
using ECA.Business.Queries.Models.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Business.Queries.Models.Sevis;
using ECA.WebApi.Custom.Storage;
using System.Net.Http;

namespace ECA.WebApi.Test.Controllers.Persons
{
    [TestClass]
    public class ParticipantPersonsSevisControllerTest
    {
        private ParticipantPersonsSevisController controller;
        private Mock<IParticipantPersonsSevisService> participantPersonSevisService;
        private Mock<IUserProvider> userProvider;
        private Mock<IFileStorageHandler> storageHandler;

        [TestInitialize]
        public void TestInit()
        {
            participantPersonSevisService = new Mock<IParticipantPersonsSevisService>();
            userProvider = new Mock<IUserProvider>();
            storageHandler = new Mock<IFileStorageHandler>();
            controller = new ParticipantPersonsSevisController(participantPersonSevisService.Object, userProvider.Object, storageHandler.Object);
        }

        [TestMethod]
        public async Task TestGetParticipantPersonsSevisCommStatusesByIdAsync()
        {
            var projectId = 1;
            var participantId = 2;
            var model = new PagingQueryBindingModel<ParticipantPersonSevisCommStatusDTO>();
            var response = await controller.GetParticipantPersonsSevisCommStatusesByIdAsync(projectId, participantId, model);
            participantPersonSevisService.Verify(x => x.GetSevisCommStatusesByParticipantIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QueryableOperator<ParticipantPersonSevisCommStatusDTO>>()), Times.Once());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<ParticipantPersonSevisCommStatusDTO>>));
        }

        [TestMethod]
        public async Task TestGetParticipantPersonsSevisCommStatusesByIdAsync_InvalidModel()
        {
            var projectId = 1;
            var participantId = 2;
            var model = new PagingQueryBindingModel<ParticipantPersonSevisCommStatusDTO>();
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetParticipantPersonsSevisCommStatusesByIdAsync(projectId, participantId, model);
            participantPersonSevisService.Verify(x => x.GetSevisCommStatusesByParticipantIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QueryableOperator<ParticipantPersonSevisCommStatusDTO>>()), Times.Never());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
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
            Func<Task> f = () => controller.PostSendToSevisAsync(1, projectId, model);
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

        [TestMethod]
        public async Task TestGetSevisParticipantsByProjectIdAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetSevisParticipantsByProjectIdAsync(1, new PagingQueryBindingModel<ParticipantPersonSevisDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestGetSevisParticipantsByProjectIdAsync()
        {
            participantPersonSevisService.Setup(x => x.GetSevisParticipantsByProjectIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<ParticipantPersonSevisDTO>>()))
                .ReturnsAsync(new PagedQueryResults<ParticipantPersonSevisDTO>(1, new List<ParticipantPersonSevisDTO>()));
            var response = await controller.GetSevisParticipantsByProjectIdAsync(1, new PagingQueryBindingModel<ParticipantPersonSevisDTO>());
            participantPersonSevisService.Verify(x => x.GetSevisParticipantsByProjectIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<ParticipantPersonSevisDTO>>()), Times.Once());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<ParticipantPersonSevisDTO>>));
        }
        [TestMethod]
        public async Task GetSevisBatchProcessingInfoAsync()
        {
            userProvider.Setup(x => x.GetCurrentUser()).Returns(new DebugWebApiUser());
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new User(20));

            participantPersonSevisService.Setup(x => x.GetBatchInfoByBatchIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(new SevisBatchInfoDTO());

            var response = await controller.GetSevisBatchProcessingInfoAsync(1, 2, "batchId");
            participantPersonSevisService.Verify(x => x.GetBatchInfoByBatchIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()), Times.Once());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<SevisBatchInfoDTO>));
        }

        [TestMethod]
        public async Task GetDS2019FileAsync()
        {
            participantPersonSevisService.Setup(x => x.GetDS2019FileNameAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync("fileName.pdf");
            storageHandler.Setup(x => x.BlobExistsAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(true);
            storageHandler.Setup(x => x.GetFileAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            var response = await controller.GetDS2019FileAsync(1, 1);
            storageHandler.Verify(x => x.BlobExistsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            storageHandler.Verify(x => x.GetFileAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task GetDS2019FileAsync_NullFileName()
        {
            participantPersonSevisService.Setup(x => x.GetDS2019FileNameAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(null);
            controller.Request = new HttpRequestMessage();
            var response = await controller.GetDS2019FileAsync(1, 1);
            Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }

        [TestMethod]
        public async Task GetDS2019FileAsync_BlobDoesNotExist()
        {
            participantPersonSevisService.Setup(x => x.GetDS2019FileNameAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync("fileName.pdf");
            storageHandler.Setup(x => x.BlobExistsAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);
            controller.Request = new HttpRequestMessage();
            var response = await controller.GetDS2019FileAsync(1, 1);
            storageHandler.Verify(x => x.BlobExistsAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
            Assert.IsInstanceOfType(response, typeof(HttpResponseMessage));
            Assert.AreEqual(response.StatusCode, HttpStatusCode.NotFound);
        }
    }
}
