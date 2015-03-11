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

namespace ECA.WebApi.Test.Controllers.Admin
{
    [TestClass]
    public class MoneyFlowsControllerTest
    {
        private Mock<IMoneyFlowService> mock;
        private MoneyFlowsController controller;

        [TestInitialize]
        public void TestInit() 
        {
            mock = new Mock<IMoneyFlowService>();
            controller = new MoneyFlowsController(mock.Object);
            ControllerHelper.InitializeController(controller);
        }

        #region Get moneyflows by project
        [TestMethod]
        public async Task TestGetMoneyFlowsByProjectAsync()
        {
            mock.Setup(x => x.GetMoneyFlowsByProjectIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<MoneyFlowDTO>>()))
                .ReturnsAsync(new PagedQueryResults<MoneyFlowDTO>(1, new List<MoneyFlowDTO>()));
            var response = await controller.GetMoneyFlowsByProjectId(1, new MultipleFilterBindingModel());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<MoneyFlowDTO>>));
        }

        [TestMethod]
        public async Task TestGetMoneyFlowsByProjectAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetMoneyFlowsByProjectId(1, new MultipleFilterBindingModel());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
