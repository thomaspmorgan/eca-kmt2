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
    public class GoalsControllerTest
    {
        private Mock<IGoalService> serviceMock;
        private GoalsController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IGoalService>();
            serviceMock.Setup(x => x.GetAsync(It.IsAny<QueryableOperator<GoalDTO>>()))
                .ReturnsAsync(new PagedQueryResults<GoalDTO>(1, new List<GoalDTO>()));
            controller = new GoalsController(serviceMock.Object);
            ControllerHelper.InitializeController(controller);
        }

        #region Get
        [TestMethod]
        public async Task TestGetThemesAsync()
        {
            var response = await controller.GetLocationsAsync(new PagingQueryBindingModel<GoalDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<GoalDTO>>));
        }

        [TestMethod]
        public async Task TestGetThemesAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetLocationsAsync(new PagingQueryBindingModel<GoalDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
