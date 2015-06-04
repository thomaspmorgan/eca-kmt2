using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ECA.Business.Service.Admin;
using ECA.WebApi.Controllers.Admin;
using System.Threading.Tasks;
using ECA.Core.DynamicLinq;
using ECA.Business.Queries.Models.Admin;
using ECA.WebApi.Models.Query;
using System.Collections.Generic;
using System.Web.Http.Results;
using ECA.Core.Query;

namespace ECA.WebApi.Test.Controllers.Admin
{
    [TestClass]
    public class OrganizationsControllerTest
    {
        private Mock<IOrganizationService> mock;
        private OrganizationsController controller;

        [TestInitialize]
        public void TestInit()
        {
            mock = new Mock<IOrganizationService>();
            mock.Setup(x => x.GetOrganizationsAsync(It.IsAny<QueryableOperator<SimpleOrganizationDTO>>()))
                .ReturnsAsync(new PagedQueryResults<SimpleOrganizationDTO>(1, new List<SimpleOrganizationDTO>()));
            
            controller = new OrganizationsController(mock.Object);
        }

        [TestMethod]
        public async Task TestGetOrganizationByIdAsync()
        {
            mock.Setup(x => x.GetOrganizationByIdAsync(It.IsAny<int>())).ReturnsAsync(new OrganizationDTO());
            var response = await controller.GetOrganizationByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<OrganizationDTO>));
        }

        [TestMethod]
        public async Task TestGetOrganizationByIdAsync_DoesNotExist()
        {
            mock.Setup(x => x.GetOrganizationByIdAsync(It.IsAny<int>())).ReturnsAsync(null);
            var response = await controller.GetOrganizationByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task TestGetOrganizationsAsync()
        {
            var response = await controller.GetOrganizationsAsync(new PagingQueryBindingModel<SimpleOrganizationDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimpleOrganizationDTO>>));
        }

        [TestMethod]
        public async Task TestGetOrganizationAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetOrganizationsAsync(new PagingQueryBindingModel<SimpleOrganizationDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
    }
}
