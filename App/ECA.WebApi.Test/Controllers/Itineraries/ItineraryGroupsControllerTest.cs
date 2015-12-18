using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ECA.Business.Service.Itineraries;
using ECA.WebApi.Security;
using ECA.WebApi.Controllers.Itineraries;
using ECA.Business.Models.Itineraries;
using ECA.WebApi.Models.Query;
using System.Threading.Tasks;
using ECA.Core.Query;
using ECA.Core.DynamicLinq;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Itineraries
{
    [TestClass]
    public class ItineraryGroupsControllerTest
    {
        private Mock<IItineraryGroupService> service;
        private Mock<IUserProvider> userProvider;
        private ItineraryGroupsController controller;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            service = new Mock<IItineraryGroupService>();

            controller = new ItineraryGroupsController(service.Object, userProvider.Object);
        }
        #region Get
        [TestMethod]
        public async Task TestGetItinerariesByProjectIdAsync()
        {
            var projectId = 1;
            var itineraryId = 2;
            var model = new PagingQueryBindingModel<ItineraryGroupDTO>();
            var results = await controller.GetItinerariesByProjectIdAsync(projectId, itineraryId, model);
            Assert.IsInstanceOfType(results, typeof(OkNegotiatedContentResult<PagedQueryResults<ItineraryGroupDTO>>));
            service.Verify(x => x.GetItineraryGroupsByItineraryIdAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<QueryableOperator<ItineraryGroupDTO>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetItinerariesByProjectIdAsync_InvalidModel()
        {
            var projectId = 1;
            var itineraryId = 2;
            var model = new PagingQueryBindingModel<ItineraryGroupDTO>();
            controller.ModelState.AddModelError("key", "error");
            var results = await controller.GetItinerariesByProjectIdAsync(projectId, itineraryId, model);
            Assert.IsInstanceOfType(results, typeof(InvalidModelStateResult));            
        }
        #endregion
    }
}
