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
        private Mock<ISocialMediaPresenceModelHandler> socialMediaHandler;
        private OrganizationsController controller;

        [TestInitialize]
        public void TestInit()
        {
            organizationService = new Mock<IOrganizationService>();
            organizationTypeService = new Mock<IOrganizationTypeService>();
            userProvider = new Mock<IUserProvider>();
            addressHandler = new Mock<IAddressModelHandler>();
            socialMediaHandler = new Mock<ISocialMediaPresenceModelHandler>();
            organizationService.Setup(x => x.GetOrganizationsAsync(It.IsAny<QueryableOperator<SimpleOrganizationDTO>>()))
                .ReturnsAsync(new PagedQueryResults<SimpleOrganizationDTO>(1, new List<SimpleOrganizationDTO>()));
            
            controller = new OrganizationsController(organizationService.Object, organizationTypeService.Object, userProvider.Object, addressHandler.Object, socialMediaHandler.Object);
        }

        [TestMethod]
        public async Task TestGetOrganizationTypesAsync()
        {
            var response = await controller.GetOrganizationTypesAsync(new PagingQueryBindingModel<OrganizationTypeDTO>());
            organizationTypeService.Verify(x => x.GetAsync(It.IsAny<QueryableOperator<OrganizationTypeDTO>>()), Times.Once());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<OrganizationTypeDTO>>));
        }

        [TestMethod]
        public async Task TestGetOrganizationTypesAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetOrganizationTypesAsync(new PagingQueryBindingModel<OrganizationTypeDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
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
        public async Task TestPostOrganizationAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(0));
            organizationService.Setup(x => x.CreateAsync(It.IsAny<NewOrganization>()))
                .ReturnsAsync(new Organization());
            organizationService.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var response = await controller.PostOrganizationAsync(new CreateOrganizationBindingModel());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<OrganizationDTO>));
            organizationService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestPostOrganizationAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var model = new CreateOrganizationBindingModel();
            var response = await controller.PostOrganizationAsync(model);
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
            var response = await controller.PostAddressAsync(1, model);
            addressHandler.Verify(x => x.HandleAdditionalAddressAsync<Organization>(It.IsAny<AddressBindingModelBase<Organization>>(), It.IsAny<ApiController>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPutAddressAsync()
        {
            var model = new UpdatedAddressBindingModel();
            var response = await controller.PutAddressAsync(1, model);
            addressHandler.Verify(x => x.HandleUpdateAddressAsync(It.IsAny<UpdatedAddressBindingModel>(), It.IsAny<ApiController>()), Times.Once());
        }

        [TestMethod]
        public async Task TestDeleteAddressAsync()
        {
            var response = await controller.DeleteAddressAsync(1, 2);
            addressHandler.Verify(x => x.HandleDeleteAddressAsync(It.IsAny<int>(), It.IsAny<ApiController>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostSocialMediaAsync()
        {
            var model = new OrganizationSocialMediaPresenceBindingModel();
            var response = await controller.PostSocialMediaAsync(1, model);
            socialMediaHandler.Verify(x => x.HandleSocialMediaPresenceAsync<Organization>(It.IsAny<SocialMediaBindingModelBase<Organization>>(), It.IsAny<ApiController>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPutSocialMediaAsync()
        {
            var model = new UpdatedSocialMediaBindingModel();
            var response = await controller.PutUpdateSocialMediaAsync(1, model);
            socialMediaHandler.Verify(x => x.HandleUpdateSocialMediaAsync(It.IsAny<UpdatedSocialMediaBindingModel>(), It.IsAny<ApiController>()), Times.Once());
        }

        [TestMethod]
        public async Task TestDeleteSocialMediaAsync()
        {
            var response = await controller.DeleteSocialMediaAsync(1, 1);
            socialMediaHandler.Verify(x => x.HandleDeleteSocialMediaAsync(It.IsAny<int>(), It.IsAny<ApiController>()), Times.Once());
        }
    }
}

