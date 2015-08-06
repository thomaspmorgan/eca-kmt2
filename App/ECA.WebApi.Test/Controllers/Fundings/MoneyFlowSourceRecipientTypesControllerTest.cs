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
    public class MoneyFlowSourceRecipieintTypesControllerTest
    {

        private Mock<IMoneyFlowSourceRecipientTypeService> serviceMock;
        private MoneyFlowSourceRecipientTypesController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IMoneyFlowSourceRecipientTypeService>();
            serviceMock.Setup(x => x.GetAsync(It.IsAny<QueryableOperator<MoneyFlowSourceRecipientTypeDTO>>()))
                .ReturnsAsync(new PagedQueryResults<MoneyFlowSourceRecipientTypeDTO>(1, new List<MoneyFlowSourceRecipientTypeDTO>()));

            controller = new MoneyFlowSourceRecipientTypesController(serviceMock.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetMoneyFlowSourceRecipientTypesAsync()
        {
            var response = await controller.GetMoneyFlowSourceRecipientTypesAsync(new PagingQueryBindingModel<MoneyFlowSourceRecipientTypeDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<MoneyFlowSourceRecipientTypeDTO>>));
        }

        [TestMethod]
        public async Task TestGetMoneyFlowSourceRecipientTypesAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetMoneyFlowSourceRecipientTypesAsync(new PagingQueryBindingModel<MoneyFlowSourceRecipientTypeDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
