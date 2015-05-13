using ECA.Business.Service.Admin;
using ECA.Business.Service.Lookup;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using ECA.WebApi.Controllers.Admin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECA.WebApi.Models.Query;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Admin
{
    [TestClass]
    public class GendersControllerTest
    {
        private Mock<IGenderService> mock;
        private GendersController controller;

        [TestInitialize]
        public void TestInit()
        {
            mock = new Mock<IGenderService>();
            controller = new GendersController(mock.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetGendersAsync()
        {
            mock.Setup(x => x.GetAsync(It.IsAny<QueryableOperator<SimpleLookupDTO>>()))
                .ReturnsAsync(new PagedQueryResults<SimpleLookupDTO>(1, new List<SimpleLookupDTO>()));
            var response = await controller.GetGenders(new PagingQueryBindingModel<SimpleLookupDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimpleLookupDTO>>));
        }

        [TestMethod]
        public async Task TestGetGendersAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetGenders(new PagingQueryBindingModel<SimpleLookupDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
