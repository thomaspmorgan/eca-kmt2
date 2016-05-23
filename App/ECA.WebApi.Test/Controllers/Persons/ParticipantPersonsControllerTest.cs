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
using ECA.WebApi.Security;
using ECA.Business.Service;
using ECA.WebApi.Models.Person;
using ECA.Data;

namespace ECA.WebApi.Test.Controllers.Persons
{
    [TestClass]
    public class ParticipantPersonsControllerTest
    {
        private Mock<IParticipantPersonService> service;
        private Mock<IUserProvider> userProvider;
        private Mock<IParticipantService> participantService;
        private ParticipantPersonsController controller;

        [TestInitialize]
        public void TestInit()
        {
            service = new Mock<IParticipantPersonService>();
            participantService = new Mock<IParticipantService>();
            service.Setup(x => x.GetParticipantPersonsByProjectIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<SimpleParticipantPersonDTO>>()))
               .ReturnsAsync(new PagedQueryResults<SimpleParticipantPersonDTO>(1, new List<SimpleParticipantPersonDTO>()));

            userProvider = new Mock<IUserProvider>();

            controller = new ParticipantPersonsController(service.Object, participantService.Object, userProvider.Object);
        }

        #region Get

        [TestMethod]
        public async Task TestGetParticipantPersonByIdAsync()
        {
            service.Setup(x => x.GetParticipantPersonByIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new SimpleParticipantPersonDTO());
            var response = await controller.GetParticipantPersonByIdAsync(1, 1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<SimpleParticipantPersonDTO>));
            service.Verify(x => x.GetParticipantPersonByIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
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
        public async Task TestGetIsParticipantPersonLockedAsync()
        {
            service.Setup(x => x.GetIsParticipantPersonLockedAsync(It.IsAny<int>()))
                .ReturnsAsync(false);
            var response = await controller.GetIsParticipantPersonLockedAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<bool>));
        }

        #endregion

        #region Update

        [TestMethod]
        public async Task TestPutUpdateParticipantPersonAsync()
        {
            var user = new DebugWebApiUser();
            var businessUser = new User(1);
            userProvider.Setup(x => x.GetCurrentUser()).Returns(user);
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(businessUser);

            var model = new UpdatedParticipantPersonBindingModel();
            model.ParticipantTypeId = ParticipantType.Individual.Id;

            var result = await controller.PutCreateOrUpdateParticipantPersonAsync(1, model);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<SimpleParticipantPersonDTO>));
            service.Verify(x => x.CreateOrUpdateAsync(It.IsAny<UpdatedParticipantPerson>()), Times.Once());
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
            service.Verify(x => x.GetParticipantPersonByIdAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
        }

        [TestMethod]
        public async Task TestPutUpdateParticipantPersonAsync_InvalidModel()
        {
            var model = new UpdatedParticipantPersonBindingModel();
            model.ParticipantTypeId = ParticipantType.Individual.Id;

            controller.ModelState.AddModelError("key", "error");
            var result = await controller.PutCreateOrUpdateParticipantPersonAsync(1, model);
            Assert.IsInstanceOfType(result, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
