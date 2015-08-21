using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using Moq;
using ECA.WebApi.Security;
using ECA.WebApi.Controllers.Admin;
using ECA.WebApi.Models.Admin;
using ECA.Data;
using System.Threading.Tasks;
using ECA.Business.Queries.Models.Admin;
using System.Web.Http.Results;
using ECA.Business.Service.Lookup;
using ECA.Core.Query;
using ECA.WebApi.Models.Query;

namespace ECA.WebApi.Test.Controllers.Admin
{
    [TestClass]
    public class SocialMediasControllerTest
    {
        private Mock<IUserProvider> userProvider;
        private Mock<ISocialMediaService> socialMediaService;
        private SocialMediasController controller;
        private Mock<ISocialMediaTypeService> socialMediaTypeService;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            socialMediaService = new Mock<ISocialMediaService>();
            socialMediaTypeService = new Mock<ISocialMediaTypeService>();
            controller = new SocialMediasController(userProvider.Object, socialMediaTypeService.Object, socialMediaService.Object);
        }

        [TestMethod]
        public async Task TestGetSocialMediaTypesAsync()
        {
            var response = await controller.GetSocialMediaTypesAsync(new PagingQueryBindingModel<SocialMediaTypeDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SocialMediaTypeDTO>>));
        }

        [TestMethod]
        public async Task TestGetSocialMediaTypesAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetSocialMediaTypesAsync(new PagingQueryBindingModel<SocialMediaTypeDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
    }
}
