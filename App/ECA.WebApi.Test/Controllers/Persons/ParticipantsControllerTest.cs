using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.WebApi.Controllers.Persons;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Persons
{
    [TestClass]
    public class ParticipantsControllerTest
    {
        private Mock<IParticipantService> serviceMock;
        private Mock<IUserProvider> userProvider;
        private ParticipantsController controller;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            serviceMock = new Mock<IParticipantService>();
            serviceMock.Setup(x => x.GetParticipantsAsync(It.IsAny<QueryableOperator<SimpleParticipantDTO>>()))
                .ReturnsAsync(new PagedQueryResults<SimpleParticipantDTO>(1, new List<SimpleParticipantDTO>()));

            serviceMock.Setup(x => x.GetParticipantsByProjectIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<SimpleParticipantDTO>>())).
                ReturnsAsync(new PagedQueryResults<SimpleParticipantDTO>(1, new List<SimpleParticipantDTO>()));

            controller = new ParticipantsController(serviceMock.Object, userProvider.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetParticipantsByProjectIdAsync()
        {
            var response = await controller.GetParticipantsByProjectIdAsync(1, new PagingQueryBindingModel<SimpleParticipantDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimpleParticipantDTO>>));
            serviceMock.Verify(x => x.GetParticipantsByProjectIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<SimpleParticipantDTO>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetParticipantsByProjectIdAsyncAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetParticipantsByProjectIdAsync(1, new PagingQueryBindingModel<SimpleParticipantDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestGetParticipantByIdAsync()
        {
            serviceMock.Setup(x => x.GetParticipantByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ParticipantDTO());
            var response = await controller.GetParticipantByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ParticipantDTO>));
            serviceMock.Verify(x => x.GetParticipantByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetParticipantByIdAsync_ParticipantDoesNotExist()
        {
            serviceMock.Setup(x => x.GetParticipantByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null);
            var response = await controller.GetParticipantByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }
        #endregion

        #region Delete
        [TestMethod]
        public async Task TestDeleteParticipantAsync()
        {
            var participantId = 1;
            var projectId = 2;
            await controller.DeleteParticipantAsync(projectId, participantId);
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            serviceMock.Verify(x => x.DeleteAsync(It.IsAny<DeletedParticipant>()), Times.Once());
            serviceMock.Verify(x => x.SaveChangesAsync(), Times.Once());
        }
        #endregion
    }
}
