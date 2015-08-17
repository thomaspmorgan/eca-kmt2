using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Data;
using ECA.WebApi.Controllers.Admin;
using ECA.WebApi.Controllers.Programs;
using ECA.WebApi.Models.Admin;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Admin
{
    [TestClass]
    public class LocationsControllerTest
    {
        private Mock<ILocationService> serviceMock;
        private Mock<IUserProvider> userProvider;
        private LocationsController controller;        

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            serviceMock = new Mock<ILocationService>();
            serviceMock.Setup(x => x.GetAsync(It.IsAny<QueryableOperator<LocationDTO>>()))
                .ReturnsAsync(new PagedQueryResults<LocationDTO>(1, new List<LocationDTO>()));
            controller = new LocationsController(serviceMock.Object, userProvider.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetLocationsAsync()
        {
            var response = await controller.GetLocationsAsync(new PagingQueryBindingModel<LocationDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<LocationDTO>>));
        }

        [TestMethod]
        public async Task TestGetLocationsAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetLocationsAsync(new PagingQueryBindingModel<LocationDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Post
        [TestMethod]
        public async Task TestPostCreateLocationAsync()
        {
            var model = new LocationBindingModel
            {
                LocationTypeId = LocationType.Building.Id,
            };
            serviceMock.Setup(x => x.CreateAsync(It.IsAny<AdditionalLocation>())).ReturnsAsync(new Location());
            serviceMock.Setup(x => x.GetLocationByIdAsync(It.IsAny<int>())).ReturnsAsync(new LocationDTO());
            var response = await controller.PostCreateLocationAsync(model);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<LocationDTO>));
            serviceMock.Verify(x => x.CreateAsync(It.IsAny<AdditionalLocation>()), Times.Once());
            serviceMock.Verify(x => x.SaveChangesAsync(), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostCreateLocationAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var model = new LocationBindingModel
            {
                LocationTypeId = LocationType.Building.Id,
            };
            var response = await controller.PostCreateLocationAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        
        #endregion

        #region Put
        [TestMethod]
        public async Task TestPutUpdateLocationAsync()
        {
            var model = new UpdatedLocationBindingModel
            {
                LocationTypeId = LocationType.Building.Id,
            };
            serviceMock.Setup(x => x.GetLocationByIdAsync(It.IsAny<int>())).ReturnsAsync(new LocationDTO());
            var response = await controller.PutUpdateLocationAsync(model);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<LocationDTO>));
            serviceMock.Verify(x => x.UpdateAsync(It.IsAny<UpdatedLocation>()), Times.Once());
            serviceMock.Verify(x => x.SaveChangesAsync(), Times.Once());
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
        }

        [TestMethod]
        public async Task TestUpdateCreateLocationAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var model = new UpdatedLocationBindingModel
            {
                LocationTypeId = LocationType.Building.Id,
            };
            var response = await controller.PutUpdateLocationAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        
        #endregion
    }
}
