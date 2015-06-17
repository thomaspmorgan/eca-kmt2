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
    public class ParticipantTypesControllerTest
    {
        private Mock<IParticipantTypeService> serviceMock;
        private ParticipantTypesController controller;        

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IParticipantTypeService>();
            serviceMock.Setup(x => x.GetAsync(It.IsAny<QueryableOperator<ParticipantTypeDTO>>()))
                .ReturnsAsync(new PagedQueryResults<ParticipantTypeDTO>(1, new List<ParticipantTypeDTO>()));
            controller = new ParticipantTypesController(serviceMock.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetAsync()
        {
            var response = await controller.GetAsync(new PagingQueryBindingModel<ParticipantTypeDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<ParticipantTypeDTO>>));
        }

        [TestMethod]
        public async Task TestGetAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetAsync(new PagingQueryBindingModel<ParticipantTypeDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
