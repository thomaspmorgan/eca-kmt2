using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Persons;
using ECA.WebApi.Controllers.Persons;
using Moq;
using ECA.Core.DynamicLinq;
using ECA.Business.Queries.Models.Persons;
using System.Collections.Generic;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Persons
{
    [TestClass]
    public class ParticipantPersonsControllerTest
    {
        private Mock<IParticipantPersonService> serviceMock;
        private ParticipantPersonsController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IParticipantPersonService>();
            serviceMock.Setup(x => x.GetParticipantPersonsAsync(It.IsAny<QueryableOperator<SimpleParticipantPersonDTO>>()))
               .ReturnsAsync(new PagedQueryResults<SimpleParticipantPersonDTO>(1, new List<SimpleParticipantPersonDTO>()));
            serviceMock.Setup(x => x.GetParticipantPersonsByProjectIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<SimpleParticipantPersonDTO>>()))
               .ReturnsAsync(new PagedQueryResults<SimpleParticipantPersonDTO>(1, new List<SimpleParticipantPersonDTO>()));
            serviceMock.Setup(x => x.GetParticipantPersonByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new SimpleParticipantPersonDTO());
            controller = new ParticipantPersonsController(serviceMock.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetParticipantPersonsAsync()
        {
            var response = await controller.GetParticipantPersonsAsync(new PagingQueryBindingModel<SimpleParticipantPersonDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimpleParticipantPersonDTO>>));
        }

        [TestMethod]
        public async Task TestGetParticipantPersonsAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetParticipantPersonsAsync(new PagingQueryBindingModel<SimpleParticipantPersonDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }


        [TestMethod]
        public async Task TestGetParticipantPersonsByProjectIdAsync()
        {
            var response = await controller.GetParticipantPersonsByProjectIdAsync(1, new PagingQueryBindingModel<SimpleParticipantPersonDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimpleParticipantPersonDTO>>));
        }

        [TestMethod]
        public async Task TestGetParticipantPersonsByProjectIdAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetParticipantPersonsByProjectIdAsync(1, new PagingQueryBindingModel<SimpleParticipantPersonDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestGetParticipantPersonByIdAsync()
        {
            var response = await controller.GetParticipantPersonByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<SimpleParticipantPersonDTO>));
        }

        [TestMethod]
        public async Task TestGetParticipantPersonByIdAsync_NotFound()
        {
            serviceMock.Setup(x => x.GetParticipantPersonByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null);
            var response = await controller.GetParticipantPersonByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }
        #endregion
    }
}
