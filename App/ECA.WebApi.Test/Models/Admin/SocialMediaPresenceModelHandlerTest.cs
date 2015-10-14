using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.Business.Service.Admin;
using Moq;
using ECA.WebApi.Security;
using ECA.WebApi.Models.Admin;
using System.Threading.Tasks;
using ECA.Data;
using ECA.WebApi.Test.Security;
using System.Web.Http.Results;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Models.Admin
{
    [TestClass]
    public class SocialMediaPresenceModelHandlerTest
    {
        private Mock<ISocialMediaService> socialMediaService;
        private Mock<IUserProvider> userProvider;
        private SocialMediaPresenceModelHandler handler;
        private TestController controller;

        [TestInitialize]
        public void TestInit()
        {
            socialMediaService = new Mock<ISocialMediaService>();
            userProvider = new Mock<IUserProvider>();
            controller = new TestController();
            handler = new SocialMediaPresenceModelHandler(socialMediaService.Object, userProvider.Object);
        }

        [TestMethod]
        public async Task TestHandleSocialMediaPresenceAsync()
        {
            userProvider.Setup(x => x.GetCurrentUser()).Returns(new DebugWebApiUser());
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));
            socialMediaService.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new SocialMediaDTO());
            socialMediaService.Setup(x => x.CreateAsync(It.IsAny<SocialMediaPresence<Organization>>())).ReturnsAsync(new SocialMedia());

            var model = new OrganizationSocialMediaPresenceBindingModel();
            model.SocialMediaTypeId = SocialMediaType.Facebook.Id;
            var response = await handler.HandleSocialMediaPresenceAsync<Organization>(model, controller);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<SocialMediaDTO>));

            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            socialMediaService.Verify(x => x.CreateAsync(It.IsAny<SocialMediaPresence<Organization>>()), Times.Once());
            socialMediaService.Verify(x => x.SaveChangesAsync(), Times.Once());
            socialMediaService.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestHandleSocialMediaPresenceAsync_ModelIsInvalid()
        {
            controller.ModelState.AddModelError("key", "error");
            var model = new OrganizationSocialMediaPresenceBindingModel();
            var response = await handler.HandleSocialMediaPresenceAsync<Organization>(model, controller);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));

        }

        [TestMethod]
        public async Task TestHandleUpdateSocialMediaAsync()
        {
            var model = new UpdatedSocialMediaBindingModel
            {
                SocialMediaTypeId = SocialMediaType.Facebook.Id
            };
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new User(1));
            var response = await handler.HandleUpdateSocialMediaAsync(model, controller);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<SocialMediaDTO>));
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            socialMediaService.Verify(x => x.UpdateAsync(It.IsAny<UpdatedSocialMediaPresence>()), Times.Once());
            socialMediaService.Verify(x => x.SaveChangesAsync(), Times.Once());
            socialMediaService.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestHandleUpdateSocialMediaAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var model = new UpdatedSocialMediaBindingModel
            {

            };
            var response = await handler.HandleUpdateSocialMediaAsync(model, controller);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestHandleDeleteSocialMediaAsync()
        {
            var response = await handler.HandleDeleteSocialMediaAsync(1, controller);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            socialMediaService.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once());
            socialMediaService.Verify(x => x.SaveChangesAsync(), Times.Once());
        }
    }
}
