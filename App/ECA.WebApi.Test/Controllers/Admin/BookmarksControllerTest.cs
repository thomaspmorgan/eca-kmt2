using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ECA.WebApi.Controllers.Admin;
using ECA.WebApi.Security;
using Moq;
using ECA.Business.Service.Admin;
using System.Threading.Tasks;
using ECA.WebApi.Models.Admin;
using System.Web.Http.Results;
using System.Security.Claims;
using ECA.Business.Service;

namespace ECA.WebApi.Test.Controllers.Admin
{
    [TestClass]
    public class BookmarksControllerTest
    {

        private Mock<IUserProvider> userProvider;
        private Mock<IBookmarkService> service;
        private BookmarksController controller;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            service = new Mock<IBookmarkService>();
            controller = new BookmarksController(userProvider.Object, service.Object);
        }

        [TestMethod]
        public async Task TestDeleteBookmarkAsync()
        {
            var response = await controller.DeleteBookmarkAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            service.Verify(x => x.DeleteBookmarkAsync(It.IsAny<int>()), Times.Once());
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestPostBookmarkAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new User(1));
            var model = new BookmarkBindingModel
            {
                ProgramId = 1
            };

            var response = await controller.PostBookmarkAsync(model);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            userProvider.Verify(x => x.GetCurrentUser(), Times.Once());
            userProvider.Verify(x => x.GetBusinessUser(It.IsAny<IWebApiUser>()), Times.Once());
            service.Verify(x => x.CreateBookmarkAsync(It.IsAny<NewBookmark>()), Times.Once());
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestPostBookmarkAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var model = new BookmarkBindingModel
            {
                ProgramId = 1
            };
            var response = await controller.PostBookmarkAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
    }
}
