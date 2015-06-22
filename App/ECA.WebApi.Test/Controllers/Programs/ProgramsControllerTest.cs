using AutoMapper;
using ECA.Business.Models.Programs;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using ECA.WebApi.Controllers.Programs;
using ECA.WebApi.Models.Programs;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using ECA.Business.Service.Admin;

namespace ECA.WebApi.Test.Controllers.Programs
{
    [TestClass]
    public class ProgramsControllerTest
    {
        private Mock<IProgramService> service;
        private Mock<IUserProvider> userProvider;
        private Mock<IFocusCategoryService> focusCategoryService;
        private Mock<IJustificationObjectiveService> justificationObjectiveService;
        private ProgramsController controller;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            service = new Mock<IProgramService>();
            focusCategoryService = new Mock<IFocusCategoryService>();
            justificationObjectiveService = new Mock<IJustificationObjectiveService>();
            service.Setup(x => x.GetProgramsAsync(It.IsAny<QueryableOperator<OrganizationProgramDTO>>()))
                .ReturnsAsync(new PagedQueryResults<OrganizationProgramDTO>(1, new List<OrganizationProgramDTO>()));
            service.Setup(x => x.CreateAsync(It.IsAny<DraftProgram>())).ReturnsAsync(new Program { RowVersion = new byte[0] });
            service.Setup(x => x.UpdateAsync(It.IsAny<EcaProgram>())).Returns(Task.FromResult<object>(null));
            service.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            service.Setup(x => x.GetProgramByIdAsync(It.IsAny<int>())).ReturnsAsync(new ProgramDTO { Id = 1, RowVersion = new byte[0] });
            controller = new ProgramsController(service.Object, userProvider.Object, focusCategoryService.Object, justificationObjectiveService.Object);
            controller.ControllerContext = ContextUtil.CreateControllerContext();
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://localhost", ""),
                new HttpResponse(new StringWriter())
                );
        }

        #region Get
        [TestMethod]
        public async Task TestGetObjectivesByProgramIdAsync()
        {
            var response = await controller.GetObjectivesByProgramIdAsync(1, new PagingQueryBindingModel<JustificationObjectiveDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<JustificationObjectiveDTO>>));
        }

        [TestMethod]
        public async Task TestGetObjectivesByProgramIdAsync_InvalidQueryModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetObjectivesByProgramIdAsync(1, new PagingQueryBindingModel<JustificationObjectiveDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestGetCategoriesByProgramIdAsync()
        {
            var response = await controller.GetCategoriesByProgramIdAsync(1, new PagingQueryBindingModel<FocusCategoryDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<FocusCategoryDTO>>));
        }

        [TestMethod]
        public async Task TestGetCategoriesByProgramIdAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetCategoriesByProgramIdAsync(1, new PagingQueryBindingModel<FocusCategoryDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestGetProgramsAsync()
        {
            var response = await controller.GetProgramsAsync(new PagingQueryBindingModel<OrganizationProgramDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<OrganizationProgramDTO>>));
        }

        [TestMethod]
        public async Task TestGetProgramsAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetProgramsAsync(new PagingQueryBindingModel<OrganizationProgramDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestGetProgramByIdAsync()
        {
            service.Setup(x => x.GetProgramByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ProgramDTO { RowVersion = new byte[0] });
            var response = await controller.GetProgramByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ProgramViewModel>));
        }

        [TestMethod]
        public async Task TestGetProgramByIdAsync_ProgramDoesNotExist()
        {
            service.Setup(x => x.GetProgramByIdAsync(It.IsAny<int>())).ReturnsAsync(null);
            var response = await controller.GetProgramByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        #endregion

        #region Post
        [TestMethod]
        public async Task TestPostProgramAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(0));
            var model = new ProgramBindingModel
            {
                Name = "name",
                Description = "desc",
                ProgramStatusId = ProgramStatus.Active.Id
            };
            var response = await controller.PostProgramAsync(model);
            service.Verify(x => x.CreateAsync(It.IsAny<DraftProgram>()), Times.Once());
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
            service.Verify(x => x.GetProgramByIdAsync(It.IsAny<int>()), Times.Once());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ProgramViewModel>));
        }

        [TestMethod]
        public async Task TestPostProgramAsync_InvalidModel()
        {
            var model = new DraftProgramBindingModel();
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostProgramAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Put
        [TestMethod]
        public async Task TestPutProgramAsync()
        {
            var user = SetDebugUser();
            var model = new ProgramBindingModel
            {
                Name = "name",
                Description = "desc",
                ProgramStatusId = ProgramStatus.Active.Id,
                RowVersion = Convert.ToBase64String(new byte[0]),
            };
            model.ProgramStatusId = ProgramStatus.Active.Id;
            userProvider.Setup(x => x.GetCurrentUser()).Returns(user);
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));
            var response = await controller.PutProgramAsync(model);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ProgramViewModel>));
            service.Verify(x => x.UpdateAsync(It.IsAny<EcaProgram>()), Times.Once());
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
            service.Verify(x => x.GetProgramByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPutProgramAsync_InvalidModel()
        {
            var model = new ProgramBindingModel();
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PutProgramAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        private DebugWebApiUser SetDebugUser()
        {
            var debugUser = new DebugWebApiUser();
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(debugUser.GetClaims(), "Bearer"));
            Thread.CurrentPrincipal = claimsPrincipal;
            HttpContext.Current.User = claimsPrincipal;
            return debugUser;
        }
    }
}
