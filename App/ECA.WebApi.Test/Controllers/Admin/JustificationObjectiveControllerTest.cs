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
    public class JustificationObjectiveControllerTest
    {
        private Mock<IJustificationObjectiveService> serviceMock;
        private JustificationObjectivesController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IJustificationObjectiveService>();
            serviceMock.Setup(x => x.GetJustificationObjectivesByOfficeIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<JustificationObjectiveDTO>>()))
                .ReturnsAsync(new PagedQueryResults<JustificationObjectiveDTO>(1, new List<JustificationObjectiveDTO>()));
            controller = new JustificationObjectivesController(serviceMock.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetJustificationObjectivesAsync()
        {
            var response = await controller.GetJustificationObjectivesAsync(1, new PagingQueryBindingModel<JustificationObjectiveDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<JustificationObjectiveDTO>>));
        }

        [TestMethod]
        public async Task TestGetJustificationObjectivesAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetJustificationObjectivesAsync(1, new PagingQueryBindingModel<JustificationObjectiveDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
