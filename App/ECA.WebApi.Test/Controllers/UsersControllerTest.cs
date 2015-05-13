using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using ECA.WebApi.Controllers;
using CAM.Business.Service;
using Moq;
using ECA.WebApi.Models.Query;
using CAM.Business.Queries.Models;
using ECA.Core.Query;
using System.Collections.Generic;
using ECA.Core.DynamicLinq;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers
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
