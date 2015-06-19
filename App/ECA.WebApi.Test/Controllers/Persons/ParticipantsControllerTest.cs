using ECA.Business.Queries.Models.Persons;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service.Persons;
using ECA.Business.Service.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.WebApi.Controllers.Persons;
using ECA.WebApi.Controllers.Programs;
using ECA.WebApi.Models.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Programs
{
    [TestClass]
    public class ParticipantsControllerTest
    {
        private Mock<IParticipantService> serviceMock;
        private ParticipantsController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IParticipantService>();
            serviceMock.Setup(x => x.GetParticipantsAsync(It.IsAny<QueryableOperator<SimpleParticipantDTO>>()))
                .ReturnsAsync(new PagedQueryResults<SimpleParticipantDTO>(1, new List<SimpleParticipantDTO>()));

            serviceMock.Setup(x => x.GetParticipantsByProjectIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<SimpleParticipantDTO>>())).
                ReturnsAsync(new PagedQueryResults<SimpleParticipantDTO>(1, new List<SimpleParticipantDTO>()));

            controller = new ParticipantsController(serviceMock.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetParticipantsAsync()
        {
            var response = await controller.GetParticipantsAsync(new PagingQueryBindingModel<SimpleParticipantDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimpleParticipantDTO>>));
            serviceMock.Verify(x => x.GetParticipantsAsync(It.IsAny<QueryableOperator<SimpleParticipantDTO>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetParticipantsAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetParticipantsAsync(new PagingQueryBindingModel<SimpleParticipantDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

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
    }
}
