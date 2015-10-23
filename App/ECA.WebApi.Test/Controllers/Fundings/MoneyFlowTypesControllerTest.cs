using ECA.Business.Queries.Models.Fundings;
using ECA.Business.Service.Fundings;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.WebApi.Controllers.Fundings;
using ECA.WebApi.Models.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Fundings
{
    [TestClass]
    public class MoneyFlowTypesControllerTest
    {

        private Mock<IMoneyFlowTypeService> serviceMock;
        private MoneyFlowTypesController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IMoneyFlowTypeService>();
            serviceMock.Setup(x => x.GetAsync(It.IsAny<QueryableOperator<MoneyFlowTypeDTO>>()))
                .ReturnsAsync(new PagedQueryResults<MoneyFlowTypeDTO>(1, new List<MoneyFlowTypeDTO>()));

            controller = new MoneyFlowTypesController(serviceMock.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetMoneyFlowTypesAsync()
        {
            var response = await controller.GetMoneyFlowTypesAsync(new PagingQueryBindingModel<MoneyFlowTypeDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<MoneyFlowTypeDTO>>));
        }

        [TestMethod]
        public async Task TestGetMoneyFlowTypesAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetMoneyFlowTypesAsync(new PagingQueryBindingModel<MoneyFlowTypeDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
