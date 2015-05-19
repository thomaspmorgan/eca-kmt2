using CAM.Business.Queries.Models;
using CAM.Business.Service;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.WebApi.Controllers.Security;
using ECA.WebApi.Models.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Security
{
    [TestClass]
    public class UsersControllerTest
    {
        private UsersController controller;
        private Mock<IUserService> userService;

        [TestInitialize]
        public void TestInit()
        {
            userService = new Mock<IUserService>();
            controller = new UsersController(userService.Object);
            controller.ControllerContext = ContextUtil.CreateControllerContext();

            userService.Setup(x => x.GetUsersAsync(It.IsAny<QueryableOperator<UserDTO>>()))
                .ReturnsAsync(new PagedQueryResults<UserDTO>(0, new List<UserDTO>()));
        }

        [TestMethod]
        public async Task TestGetUserAsync()
        {
            var response = await controller.GetUsersAsync(new PagingQueryBindingModel<UserDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<UserDTO>>));
        }

        [TestMethod]
        public async Task TestGetUserAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetUsersAsync(new PagingQueryBindingModel<UserDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
    }
}
