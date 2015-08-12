using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Fundings;
using ECA.WebApi.Controllers.Fundings;
using Moq;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Business.Queries.Models.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECA.WebApi.Models.Query;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Fundings
{
    [TestClass]
    public class MoneyFlowStatusesControllerTest
    {

        private Mock<IMoneyFlowStatusService> serviceMock;
        private MoneyFlowStatusesController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IMoneyFlowStatusService>();
            serviceMock.Setup(x => x.GetAsync(It.IsAny<QueryableOperator<MoneyFlowStatusDTO>>()))
                .ReturnsAsync(new PagedQueryResults<MoneyFlowStatusDTO>(1, new List<MoneyFlowStatusDTO>()));

            controller = new MoneyFlowStatusesController(serviceMock.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetMoneyFlowStatusesAsync()
        {
            var response = await controller.GetMoneyFlowStatusesAsync(new PagingQueryBindingModel<MoneyFlowStatusDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<MoneyFlowStatusDTO>>));
        }

        [TestMethod]
        public async Task TestGetMoneyFlowStatusesAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetMoneyFlowStatusesAsync(new PagingQueryBindingModel<MoneyFlowStatusDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
