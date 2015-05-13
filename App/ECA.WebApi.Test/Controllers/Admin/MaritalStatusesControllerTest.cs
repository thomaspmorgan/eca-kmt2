using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using Moq;
using ECA.WebApi.Controllers.Admin;
using ECA.Business.Service.Lookup;
using System.Collections.Generic;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.WebApi.Models.Query;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Admin
{
    [TestClass]
    public class MaritalStatusesControllerTest
    {

        private Mock<IMaritalStatusService> mock;
        private MaritalStatusesController controller;

        [TestInitialize]
        public void TestInit()
        {
            mock = new Mock<IMaritalStatusService>();
            controller = new MaritalStatusesController(mock.Object);
            ControllerHelper.InitializeController(controller);
        }

        [TestMethod]
        public async Task TestGetMaritalStatuesAsync()
        {
            mock.Setup(x => x.GetAsync(It.IsAny<QueryableOperator<SimpleLookupDTO>>()))
                .ReturnsAsync(new PagedQueryResults<SimpleLookupDTO>(1, new List<SimpleLookupDTO>()));
            var response = await controller.GetMaritalStatuses(new PagingQueryBindingModel<SimpleLookupDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimpleLookupDTO>>));
        }

        [TestMethod]
        public async Task TestGetGendersAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetMaritalStatuses(new PagingQueryBindingModel<SimpleLookupDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
    }
}
