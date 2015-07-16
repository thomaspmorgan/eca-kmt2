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
using ECA.Business.Service.Lookup;
using ECA.Business.Queries.Models.Lookup;
using ECA.WebApi.Security;
using ECA.WebApi.Models.Admin;
using ECA.Data;
using System.Web.Http;

namespace ECA.WebApi.Test.Controllers.Admin
{
    [TestClass]
    public class OrganizationsControllerTest
    {
        private Mock<IOrganizationService> organizationService;
        private Mock<IOrganizationTypeService> organizationTypeService;
        private Mock<IUserProvider> userProvider;
        private Mock<IAddressModelHandler> addressHandler;
        private OrganizationsController controller;

        [TestInitialize]
        public void TestInit()
        {
            organizationService = new Mock<IOrganizationService>();
            organizationTypeService = new Mock<IOrganizationTypeService>();
            userProvider = new Mock<IUserProvider>();
            addressHandler = new Mock<IAddressModelHandler>();
            organizationService.Setup(x => x.GetOrganizationsAsync(It.IsAny<QueryableOperator<SimpleOrganizationDTO>>()))
                .ReturnsAsync(new PagedQueryResults<SimpleOrganizationDTO>(1, new List<SimpleOrganizationDTO>()));
            
            controller = new OrganizationsController(organizationService.Object, organizationTypeService.Object, userProvider.Object, addressHandler.Object);
        }

        [TestMethod]
        public async Task TestGetOrganizationTypesAsync()
        {
            var response = await controller.GetOrganizationTypesAsync(new PagingQueryBindingModel<OrganizationTypeDTO>());
            organizationTypeService.Verify(x => x.GetAsync(It.IsAny<QueryableOperator<OrganizationTypeDTO>>()), Times.Once());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<OrganizationTypeDTO>>));
        }

        [TestMethod]
        public async Task TestGetOrganizationByIdAsync()
        {
            organizationService.Setup(x => x.GetOrganizationByIdAsync(It.IsAny<int>())).ReturnsAsync(new OrganizationDTO());
            var response = await controller.GetOrganizationByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<OrganizationDTO>));
            organizationService.Verify(x => x.GetOrganizationByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetOrganizationByIdAsync_DoesNotExist()
        {
            organizationService.Setup(x => x.GetOrganizationByIdAsync(It.IsAny<int>())).ReturnsAsync(null);
            var response = await controller.GetOrganizationByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
            organizationService.Verify(x => x.GetOrganizationByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetOrganizationsAsync()
        {
            var response = await controller.GetOrganizationsAsync(new PagingQueryBindingModel<SimpleOrganizationDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimpleOrganizationDTO>>));
            organizationService.Verify(x => x.GetOrganizationsAsync(It.IsAny<QueryableOperator<SimpleOrganizationDTO>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetOrganizationAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetOrganizationsAsync(new PagingQueryBindingModel<SimpleOrganizationDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestPutUpdateOrganizationAsync()
        {
            var model = new UpdatedOrganizationBindingModel();
            model.OrganizationTypeId = OrganizationType.USEducationalInstitution.Id;
            var response = await controller.PutUpdateOrganizationAsync(model);
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            organizationService.Verify(x => x.UpdateAsync(It.IsAny<EcaOrganization>()), Times.Once());
            organizationService.Verify(x => x.SaveChangesAsync(), Times.Once());
            organizationService.Verify(x => x.GetOrganizationByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPutUpdateOrganizationAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var model = new UpdatedOrganizationBindingModel();
            model.OrganizationTypeId = OrganizationType.USEducationalInstitution.Id;
            var response = await controller.PutUpdateOrganizationAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestPostAddressAsync()
        {
            var model = new OrganizationAddressBindingModel();
            var response = await controller.PostAddressAsync(model);
            addressHandler.Verify(x => x.HandleAdditionalAddress<Organization>(It.IsAny<AddressBindingModelBase<Organization>>(), It.IsAny<ApiController>()), Times.Once());
        }
    }
}

