using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Lookup;
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
    public class ParticipantStatusesControllerTest
    {
        private Mock<IParticipantStatusService> serviceMock;
        private ParticipantStatusesController controller;        

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IParticipantStatusService>();
            serviceMock.Setup(x => x.GetAsync(It.IsAny<QueryableOperator<ParticipantStatusDTO>>()))
                .ReturnsAsync(new PagedQueryResults<ParticipantStatusDTO>(1, new List<ParticipantStatusDTO>()));
            controller = new ParticipantStatusesController(serviceMock.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetAsync()
        {
            var response = await controller.GetAsync(new PagingQueryBindingModel<ParticipantStatusDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<ParticipantStatusDTO>>));
        }

        [TestMethod]
        public async Task TestGetAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetAsync(new PagingQueryBindingModel<ParticipantStatusDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
