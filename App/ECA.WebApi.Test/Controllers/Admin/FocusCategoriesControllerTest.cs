using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.WebApi.Controllers.Admin;
using ECA.WebApi.Controllers.Programs;
using ECA.WebApi.Models.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Admin
{
    [TestClass]
    public class FocusCategoriesControllerTest
    {
        private Mock<IFocusCategoryService> serviceMock;
        private FocusCategoriesController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IFocusCategoryService>();
            serviceMock.Setup(x => x.GetFocusCategoriesByOfficeIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<FocusCategoryDTO>>()))
                .ReturnsAsync(new PagedQueryResults<FocusCategoryDTO>(1, new List<FocusCategoryDTO>()));
            controller = new FocusCategoriesController(serviceMock.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetThemesAsync()
        {
            var response = await controller.GetFocusCategoriesAsync(new PagingQueryBindingModel<FocusCategoryDTO>(), 1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<FocusCategoryDTO>>));
        }

        [TestMethod]
        public async Task TestGetThemesAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetFocusCategoriesAsync(new PagingQueryBindingModel<FocusCategoryDTO>(), 1);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
