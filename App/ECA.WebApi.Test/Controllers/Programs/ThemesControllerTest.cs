using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.WebApi.Controllers.Programs;
using ECA.WebApi.Models.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Programs
{
    [TestClass]
    public class ThemesControllerTest
    {
        private Mock<IThemeService> serviceMock;
        private ThemesController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IThemeService>();
            controller = new ThemesController(serviceMock.Object);
            ControllerHelper.InitializeController(controller);
        }

        #region Get
        [TestMethod]
        public async Task TestGetThemesAsync()
        {
            serviceMock.Setup(x => x.GetThemesAsync(It.IsAny<QueryableOperator<ThemeDTO>>()))
                .Returns(Task.FromResult<PagedQueryResults<ThemeDTO>>(new PagedQueryResults<ThemeDTO>(1, new List<ThemeDTO>())));

            var response = await controller.GetThemesAsync(new PagingQueryBindingModel());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<ThemeDTO>>));
        }

        [TestMethod]
        public async Task TestGetThemesAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetThemesAsync(new PagingQueryBindingModel());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
