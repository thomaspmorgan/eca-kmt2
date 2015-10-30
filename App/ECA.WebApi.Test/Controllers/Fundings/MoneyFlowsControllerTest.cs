using ECA.Business.Models.Fundings;
using ECA.Business.Queries.Models.Fundings;
using ECA.Business.Service;
using ECA.Business.Service.Fundings;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Data;
using ECA.WebApi.Controllers.Fundings;
using ECA.WebApi.Models.Fundings;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Fundings
{
    [TestClass]
    public class MoneyFlowsControllerTest
    {
        private Mock<IMoneyFlowService> moneyFlowService;
        private Mock<IUserProvider> userProvider;
        private MoneyFlowsController controller;
        private Action<int> verifyUpdateMoneyFlowSourceRecipientEntityTypeId;
        private EditedMoneyFlow expectedMoneyFlow;

        [TestInitialize]
        public void TestInit()
        {
            expectedMoneyFlow = null;
            moneyFlowService = new Mock<IMoneyFlowService>();
            userProvider = new Mock<IUserProvider>();
            controller = new MoneyFlowsController(moneyFlowService.Object, userProvider.Object);


            Action<EditedMoneyFlow> updateOrDeleteMoneyFlowCallback = (givenMoneyFlow) =>
            {
                Assert.IsNotNull(expectedMoneyFlow, "Call SetExpectedEditedMoneyFlow in the update or delete test.");
                Assert.AreEqual(expectedMoneyFlow.Id, givenMoneyFlow.Id);
                Assert.AreEqual(expectedMoneyFlow.SourceOrRecipientEntityId, givenMoneyFlow.SourceOrRecipientEntityId);
                Assert.AreEqual(expectedMoneyFlow.SourceOrRecipientEntityTypeId, givenMoneyFlow.SourceOrRecipientEntityTypeId);
            };

            moneyFlowService.Setup(x => x.UpdateAsync(It.IsAny<UpdatedMoneyFlow>()))
                .Returns(Task.FromResult<object>(null))
                .Callback(updateOrDeleteMoneyFlowCallback);
            moneyFlowService.Setup(x => x.DeleteAsync(It.IsAny<DeletedMoneyFlow>()))
                .Returns(Task.FromResult<object>(null))
                .Callback(updateOrDeleteMoneyFlowCallback);
        }

        private void SetExpectedEditedMoneyFlow(int id, int entityId, int entityTypeId)
        {
            expectedMoneyFlow = new EditedMoneyFlow(id, entityId, entityTypeId);
        }

        #region sources
        [TestMethod]
        public async Task TestGetSourceMoneyFlowByIdAsync()
        {
            moneyFlowService.Setup(x => x.GetSourceMoneyFlowDTOByIdAsync(It.IsAny<int>())).ReturnsAsync(new SourceMoneyFlowDTO());
            var response = await controller.GetSourceMoneyFlowByIdAsync(1);
            moneyFlowService.Verify(x => x.GetSourceMoneyFlowDTOByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<SourceMoneyFlowDTO>));
        }

        [TestMethod]
        public async Task TestGetSourceMoneyFlowByIdAsync_DoesNotExist()
        {
            moneyFlowService.Setup(x => x.GetSourceMoneyFlowDTOByIdAsync(It.IsAny<int>())).ReturnsAsync(null);
            var response = await controller.GetSourceMoneyFlowByIdAsync(1);
            moneyFlowService.Verify(x => x.GetSourceMoneyFlowDTOByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }
        #endregion

        #region Get moneyflows by project
        [TestMethod]
        public async Task TestGetSourceMoneyFlowsByProjectIdAsync()
        {
            var response = await controller.GetSourceMoneyFlowsByProjectIdAsync(1);
            moneyFlowService.Verify(x => x.GetSourceMoneyFlowsByProjectIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetMoneyFlowsByProjectAsync()
        {
            moneyFlowService.Setup(x => x.GetMoneyFlowsByProjectIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<MoneyFlowDTO>>()))
                .ReturnsAsync(new PagedQueryResults<MoneyFlowDTO>(1, new List<MoneyFlowDTO>()));
            var response = await controller.GetMoneyFlowsByProjectIdAsync(1, new PagingQueryBindingModel<MoneyFlowDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<MoneyFlowDTO>>));
        }

        [TestMethod]
        public async Task TestGetMoneyFlowsByProjectAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetMoneyFlowsByProjectIdAsync(1, new PagingQueryBindingModel<MoneyFlowDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Get moneyflows by program
        [TestMethod]
        public async Task TestGetSourceMoneyFlowsByProgramIdAsync()
        {
            var response = await controller.GetSourceMoneyFlowsByProgramIdAsync(1);
            moneyFlowService.Verify(x => x.GetSourceMoneyFlowsByProgramIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetMoneyFlowsByProgramAsync()
        {
            moneyFlowService.Setup(x => x.GetMoneyFlowsByProgramIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<MoneyFlowDTO>>()))
                .ReturnsAsync(new PagedQueryResults<MoneyFlowDTO>(1, new List<MoneyFlowDTO>()));
            var response = await controller.GetMoneyFlowsByProgramIdAsync(1, new PagingQueryBindingModel<MoneyFlowDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<MoneyFlowDTO>>));
        }

        [TestMethod]
        public async Task TestGetMoneyFlowsByProgramAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetMoneyFlowsByProgramIdAsync(1, new PagingQueryBindingModel<MoneyFlowDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Get moneyflows by office
        [TestMethod]
        public async Task TestGetSourceMoneyFlowsByOfficeIdAsync()
        {
            var response = await controller.GetSourceMoneyFlowsByOfficeIdAsync(1);
            moneyFlowService.Verify(x => x.GetSourceMoneyFlowsByOfficeIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetMoneyFlowsByOfficeAsync()
        {
            moneyFlowService.Setup(x => x.GetMoneyFlowsByOfficeIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<MoneyFlowDTO>>()))
                .ReturnsAsync(new PagedQueryResults<MoneyFlowDTO>(1, new List<MoneyFlowDTO>()));
            var response = await controller.GetMoneyFlowsByOfficeIdAsync(1, new PagingQueryBindingModel<MoneyFlowDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<MoneyFlowDTO>>));
        }

        [TestMethod]
        public async Task TestGetMoneyFlowsByOfficeAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetMoneyFlowsByOfficeIdAsync(1, new PagingQueryBindingModel<MoneyFlowDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Get moneyflows by organization
        [TestMethod]
        public async Task TestGetSourceMoneyFlowsByOrganizationIdAsync()
        {
            var response = await controller.GetSourceMoneyFlowsByOrganizationIdAsync(1);
            moneyFlowService.Verify(x => x.GetSourceMoneyFlowsByOrganizationIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetMoneyFlowsByOrganizationAsync()
        {
            moneyFlowService.Setup(x => x.GetMoneyFlowsByOrganizationIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<MoneyFlowDTO>>()))
                .ReturnsAsync(new PagedQueryResults<MoneyFlowDTO>(1, new List<MoneyFlowDTO>()));
            var response = await controller.GetMoneyFlowsByOrganizationIdAsync(1, new PagingQueryBindingModel<MoneyFlowDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<MoneyFlowDTO>>));
        }

        [TestMethod]
        public async Task TestGetMoneyFlowsByOrganizationAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetMoneyFlowsByOrganizationIdAsync(1, new PagingQueryBindingModel<MoneyFlowDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Get moneyflows by person
        [TestMethod]
        public async Task TestGetMoneyFlowsByPersonAsync()
        {
            moneyFlowService.Setup(x => x.GetMoneyFlowsByPersonIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<MoneyFlowDTO>>()))
                .ReturnsAsync(new PagedQueryResults<MoneyFlowDTO>(1, new List<MoneyFlowDTO>()));
            var response = await controller.GetMoneyFlowsByPersonIdAsync(1, new PagingQueryBindingModel<MoneyFlowDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<MoneyFlowDTO>>));
        }

        [TestMethod]
        public async Task TestGetMoneyFlowsByPersonAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetMoneyFlowsByPersonIdAsync(1, new PagingQueryBindingModel<MoneyFlowDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Create
        [TestMethod]
        public async Task TestPostCreateOfficeMoneyFlowAsync()
        {
            var model = new AdditionalOfficeMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                PeerEntityTypeId = MoneyFlowSourceRecipientType.Organization.Id,
            };
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new User(1));
            var response = await controller.PostCreateOfficeMoneyFlowAsync(model);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            moneyFlowService.Verify(x => x.CreateAsync(It.IsAny<AdditionalMoneyFlow>()), Times.Once());
            moneyFlowService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestPostOfficeProjectMoneyFlowAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "value");
            var model = new AdditionalOfficeMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                PeerEntityTypeId = MoneyFlowSourceRecipientType.Organization.Id,
            };
            var response = await controller.PostCreateOfficeMoneyFlowAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestPostCreateOrganizationMoneyFlowAsync()
        {
            var model = new AdditionalOrganizationMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                PeerEntityTypeId = MoneyFlowSourceRecipientType.Organization.Id,
            };
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new User(1));
            var response = await controller.PostCreateOrganizationMoneyFlowAsync(model);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            moneyFlowService.Verify(x => x.CreateAsync(It.IsAny<AdditionalMoneyFlow>()), Times.Once());
            moneyFlowService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestPostCreateOrganizationMoneyFlowAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "value");
            var model = new AdditionalOrganizationMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                PeerEntityTypeId = MoneyFlowSourceRecipientType.Organization.Id,
            };
            var response = await controller.PostCreateOrganizationMoneyFlowAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }


        [TestMethod]
        public async Task TestPostCreateProjectMoneyFlowAsync()
        {
            var model = new AdditionalProjectMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                PeerEntityTypeId = MoneyFlowSourceRecipientType.Project.Id,
            };
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new User(1));
            var response = await controller.PostCreateProjectMoneyFlowAsync(model);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            moneyFlowService.Verify(x => x.CreateAsync(It.IsAny<AdditionalMoneyFlow>()), Times.Once());
            moneyFlowService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestPostCreateProjectMoneyFlowAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "value");
            var model = new AdditionalProjectMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                PeerEntityTypeId = MoneyFlowSourceRecipientType.Project.Id,
            };
            var response = await controller.PostCreateProjectMoneyFlowAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestPostCreateProgramMoneyFlowAsync()
        {
            var model = new AdditionalProgramMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                PeerEntityTypeId = MoneyFlowSourceRecipientType.Project.Id,
            };
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new User(1));
            var response = await controller.PostCreateProgramMoneyFlowAsync(model);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            moneyFlowService.Verify(x => x.CreateAsync(It.IsAny<AdditionalMoneyFlow>()), Times.Once());
            moneyFlowService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestPostCreateProgramMoneyFlowAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "value");
            var model = new AdditionalProgramMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
                PeerEntityTypeId = MoneyFlowSourceRecipientType.Project.Id,
            };
            var response = await controller.PostCreateProgramMoneyFlowAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        #endregion

        #region Update
        [TestMethod]
        public async Task TestPutUpdateOfficeMoneyFlowAsync()
        {
            var entityId = 1;
            var model = new UpdatedMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
            };
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new User(1));
            SetExpectedEditedMoneyFlow(model.Id, entityId, MoneyFlowSourceRecipientType.Office.Id);
            var response = await controller.PutUpdateOfficeMoneyFlowAsync(model, entityId);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            moneyFlowService.Verify(x => x.UpdateAsync(It.IsAny<UpdatedMoneyFlow>()), Times.Once());
            moneyFlowService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestPutUpdateOfficeMoneyFlowAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var entityId = 1;
            var model = new UpdatedMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
            };
            var response = await controller.PutUpdateOfficeMoneyFlowAsync(model, entityId);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestPutUpdateOrganizationMoneyFlowAsync()
        {
            var entityId = 100;
            var model = new UpdatedMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
            };
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new User(1));
            SetExpectedEditedMoneyFlow(model.Id, entityId, MoneyFlowSourceRecipientType.Organization.Id);
            var response = await controller.PutUpdateOrganizationMoneyFlowAsync(model, entityId);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            moneyFlowService.Verify(x => x.UpdateAsync(It.IsAny<UpdatedMoneyFlow>()), Times.Once());
            moneyFlowService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestPutUpdateOrganizationMoneyFlowAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var entityId = 100;
            var model = new UpdatedMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
            };
            var response = await controller.PutUpdateOrganizationMoneyFlowAsync(model, entityId);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }


        [TestMethod]
        public async Task TestPutUpdateProjectMoneyFlowAsync()
        {
            var entityId = 100;
            var model = new UpdatedMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
            };
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new User(1));
            SetExpectedEditedMoneyFlow(model.Id, entityId, MoneyFlowSourceRecipientType.Project.Id);
            var response = await controller.PutUpdateProjectMoneyFlowAsync(model, entityId);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            moneyFlowService.Verify(x => x.UpdateAsync(It.IsAny<UpdatedMoneyFlow>()), Times.Once());
            moneyFlowService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestPutUpdateProjectMoneyFlowAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var entityId = 100;
            var model = new UpdatedMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
            };
            var response = await controller.PutUpdateProjectMoneyFlowAsync(model, entityId);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestPutUpdateProgramMoneyFlowAsync()
        {
            var entityId = 100;
            var model = new UpdatedMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
            };
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new User(1));
            SetExpectedEditedMoneyFlow(model.Id, entityId, MoneyFlowSourceRecipientType.Program.Id);
            var response = await controller.PutUpdateProgramMoneyFlowAsync(model, entityId);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            moneyFlowService.Verify(x => x.UpdateAsync(It.IsAny<UpdatedMoneyFlow>()), Times.Once());
            moneyFlowService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestPutUpdateProgramMoneyFlowAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var entityId = 100;
            var model = new UpdatedMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Actual.Id,
            };
            var response = await controller.PutUpdateProgramMoneyFlowAsync(model, entityId);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Delete
        [TestMethod]
        public async Task TestDeleteOfficeMoneyFlowAsync()
        {
            var moneyFlowId = 100;
            var entityId = 200;
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new User(1));

            SetExpectedEditedMoneyFlow(moneyFlowId, entityId, MoneyFlowSourceRecipientType.Office.Id);
            var response = await controller.DeleteOfficeMoneyFlowAsync(moneyFlowId, entityId);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            moneyFlowService.Verify(x => x.DeleteAsync(It.IsAny<DeletedMoneyFlow>()), Times.Once());
            moneyFlowService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestDeleteOrganizationMoneyFlowAsync()
        {
            var moneyFlowId = 100;
            var entityId = 200;
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new User(1));
            SetExpectedEditedMoneyFlow(moneyFlowId, entityId, MoneyFlowSourceRecipientType.Organization.Id);
            var response = await controller.DeleteOrganizationMoneyFlowAsync(moneyFlowId, entityId);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            moneyFlowService.Verify(x => x.DeleteAsync(It.IsAny<DeletedMoneyFlow>()), Times.Once());
            moneyFlowService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestDeleteProjectMoneyFlowAsync()
        {
            var moneyFlowId = 1;
            var entityId = 2;
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new User(1));
            SetExpectedEditedMoneyFlow(moneyFlowId, entityId, MoneyFlowSourceRecipientType.Project.Id);
            var response = await controller.DeleteProjectMoneyFlowAsync(moneyFlowId, entityId);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            moneyFlowService.Verify(x => x.DeleteAsync(It.IsAny<DeletedMoneyFlow>()), Times.Once());
            moneyFlowService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestDeleteProgramMoneyFlowAsync()
        {
            var moneyFlowId = 100;
            var entityId = 200;
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new User(1));
            SetExpectedEditedMoneyFlow(moneyFlowId, entityId, MoneyFlowSourceRecipientType.Program.Id);
            var response = await controller.DeleteProgramMoneyFlowAsync(moneyFlowId, entityId);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            moneyFlowService.Verify(x => x.DeleteAsync(It.IsAny<DeletedMoneyFlow>()), Times.Once());
            moneyFlowService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }
        #endregion
    }
}
