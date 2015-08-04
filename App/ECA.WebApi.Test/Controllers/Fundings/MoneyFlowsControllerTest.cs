using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ECA.Business.Service.Admin;
using ECA.WebApi.Controllers.Admin;
using ECA.Business.Queries.Models.Admin;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using System.Collections.Generic;
using ECA.WebApi.Models.Query;
using System.Web.Http.Results;
using System.Threading.Tasks;
using ECA.Business.Service.Fundings;
using ECA.WebApi.Security;
using ECA.WebApi.Models.Fundings;
using ECA.Data;
using ECA.Business.Models.Fundings;
using ECA.WebApi.Controllers.Fundings;

namespace ECA.WebApi.Test.Controllers.Fundings
{
    [TestClass]
    public class MoneyFlowsControllerTest
    {
        private Mock<IMoneyFlowService> moneyFlowService;
        private Mock<IUserProvider> userProvider;
        private MoneyFlowsController controller;

        [TestInitialize]
        public void TestInit() 
        {
            moneyFlowService = new Mock<IMoneyFlowService>();
            userProvider = new Mock<IUserProvider>();
            controller = new MoneyFlowsController(moneyFlowService.Object, userProvider.Object);
        }

        #region Get moneyflows by project
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

        #region Create
        [TestMethod]
        public async Task TestPostCreateProjectMoneyFlowAsync()
        {
            var model = new AdditionalProjectMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Budgeted.Id,
                RecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
            };
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
                MoneyFlowStatusId = MoneyFlowStatus.Budgeted.Id,
                RecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
            };
            var response = await controller.PostCreateProjectMoneyFlowAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestPostCreateProgramMoneyFlowAsync()
        {
            var model = new AdditionalProgramMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Budgeted.Id,
                RecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
            };
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
                MoneyFlowStatusId = MoneyFlowStatus.Budgeted.Id,
                RecipientTypeId = MoneyFlowSourceRecipientType.Project.Id,
            };
            var response = await controller.PostCreateProgramMoneyFlowAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        
        #endregion

        #region Update
        [TestMethod]
        public async Task TestPutUpdateProjectMoneyFlowAsync()
        {
            var entityId = 1;
            var model = new UpdatedMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Budgeted.Id,                
            };
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
            var entityId = 1;
            var model = new UpdatedMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Budgeted.Id,
            };
            var response = await controller.PutUpdateProjectMoneyFlowAsync(model, entityId);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestPutUpdateProgramMoneyFlowAsync()
        {
            var entityId = 1;
            var model = new UpdatedMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Budgeted.Id,
            };
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
            var entityId = 1;
            var model = new UpdatedMoneyFlowBindingModel
            {
                MoneyFlowStatusId = MoneyFlowStatus.Budgeted.Id,
            };
            var response = await controller.PutUpdateProgramMoneyFlowAsync(model, entityId);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
