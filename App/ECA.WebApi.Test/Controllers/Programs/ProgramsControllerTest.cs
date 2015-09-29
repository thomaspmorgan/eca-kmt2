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
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using ECA.Business.Service.Admin;
using CAM.Business.Service;
using CAM.Business.Model;
using CAM.Data;
using ECA.Core.DynamicLinq.Filter;
using ECA.WebApi.Models.Security;
using System.Web.Http;

namespace ECA.WebApi.Test.Controllers.Programs
{
    [TestClass]
    public class ProgramsControllerTest
    {
        private Mock<IProgramService> service;
        private Mock<IUserProvider> userProvider;
        private Mock<IFocusCategoryService> focusCategoryService;
        private Mock<IJustificationObjectiveService> justificationObjectiveService;
        private Mock<IResourceService> resourceService;
        private Mock<IResourceAuthorizationHandler> authorizationHandler;
        private ProgramsController controller;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            service = new Mock<IProgramService>();
            focusCategoryService = new Mock<IFocusCategoryService>();
            justificationObjectiveService = new Mock<IJustificationObjectiveService>();
            resourceService = new Mock<IResourceService>();
            authorizationHandler = new Mock<IResourceAuthorizationHandler>();
            service.Setup(x => x.GetProgramsAsync(It.IsAny<QueryableOperator<OrganizationProgramDTO>>()))
                .ReturnsAsync(new PagedQueryResults<OrganizationProgramDTO>(1, new List<OrganizationProgramDTO>()));
            service.Setup(x => x.CreateAsync(It.IsAny<DraftProgram>())).ReturnsAsync(new Program { RowVersion = new byte[0] });
            service.Setup(x => x.UpdateAsync(It.IsAny<EcaProgram>())).Returns(Task.FromResult<object>(null));
            service.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            service.Setup(x => x.GetProgramByIdAsync(It.IsAny<int>())).ReturnsAsync(new ProgramDTO { Id = 1, RowVersion = new byte[0] });
            controller = new ProgramsController(service.Object, userProvider.Object, focusCategoryService.Object, justificationObjectiveService.Object, resourceService.Object, authorizationHandler.Object);
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
            justificationObjectiveService.Verify(x => x.GetJustificationObjectivesByProgramIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<JustificationObjectiveDTO>>()), Times.Once());
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
            focusCategoryService.Verify(x => x.GetFocusCategoriesByProgramIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<FocusCategoryDTO>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetCategoriesByProgramIdAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetCategoriesByProgramIdAsync(1, new PagingQueryBindingModel<FocusCategoryDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestGetProgramsHierarchyAsync()
        {
            var response = await controller.GetProgramsHierarchyAsync(new PagingQueryBindingModel<OrganizationProgramDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<OrganizationProgramDTO>>));
            service.Verify(x => x.GetProgramsHierarchyAsync(It.IsAny<QueryableOperator<OrganizationProgramDTO>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetProgramsHierarchyAsyncc_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetProgramsHierarchyAsync(new PagingQueryBindingModel<OrganizationProgramDTO>());
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
        public async Task TestGetValidParentProgramsAsync()
        {
            var response = await controller.GetValidParentProgramsAsync(1, new PagingQueryBindingModel<OrganizationProgramDTO>());            
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<OrganizationProgramDTO>>));
            service.Verify(x => x.GetValidParentProgramsAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<OrganizationProgramDTO>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetSubprogramsByProgramAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetSubprogramsByProgramAsync(1, new PagingQueryBindingModel<OrganizationProgramDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestGetSubprogramsByProgramAsync()
        {
            var response = await controller.GetSubprogramsByProgramAsync(1, new PagingQueryBindingModel<OrganizationProgramDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<OrganizationProgramDTO>>));
            service.Verify(x => x.GetSubprogramsByProgramAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<OrganizationProgramDTO>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetValidParentProgramsAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetValidParentProgramsAsync(1, new PagingQueryBindingModel<OrganizationProgramDTO>());
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

        [TestMethod]
        public async Task TestGetCollaboratorsAsync()
        {
            var programId = 10;
            var queryModel = new PagingQueryBindingModel<ResourceAuthorization>();
            queryModel.Start = 0;
            queryModel.Limit = 10;

            Action<QueryableOperator<ResourceAuthorization>> callbackTester = (op) =>
            {
                Assert.AreEqual(2, op.Filters.Count);
                var filters = op.Filters.ToList().Select(x => (ExpressionFilter<ResourceAuthorization>)x).ToList();
                var foreignResourceIdFilter = filters.Where(x => x.Property == "ForeignResourceId").FirstOrDefault();
                Assert.IsNotNull(foreignResourceIdFilter, "The foreign resource filter must exist.");
                Assert.AreEqual(ComparisonType.Equal.Value, foreignResourceIdFilter.Comparison);
                Assert.AreEqual(programId, foreignResourceIdFilter.Value);

                var resourceTypeFilter = filters.Where(x => x.Property == "ResourceTypeId").FirstOrDefault();
                Assert.IsNotNull(resourceTypeFilter, "The resource type filter must exist.");
                Assert.AreEqual(ComparisonType.Equal.Value, resourceTypeFilter.Comparison);
                Assert.AreEqual(ResourceType.Program.Id, resourceTypeFilter.Value);
            };

            resourceService.Setup(x => x.GetResourceAuthorizationsAsync(It.IsAny<QueryableOperator<ResourceAuthorization>>()))
                .ReturnsAsync(new PagedQueryResults<ResourceAuthorization>(0, new List<ResourceAuthorization>()))
                .Callback(callbackTester);

            var response = await controller.GetCollaboratorsAsync(programId, queryModel);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<ResourceAuthorization>>));
        }

        [TestMethod]
        public async Task TestGetCollaboratorsAsync_InvalidModelState()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetCollaboratorsAsync(1, new PagingQueryBindingModel<ResourceAuthorization>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestPostAddCollaboratorAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));

            var response = await controller.PostAddCollaboratorAsync(new ProgramCollaboratorBindingModel());
            authorizationHandler.Verify(x => x.HandleGrantedPermissionBindingModelAsync(It.IsAny<IGrantedPermissionBindingModel>(), It.IsAny<ApiController>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostRevokeCollaboratorAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));
            var response = await controller.PostRevokeCollaboratorAsync(new ProgramCollaboratorBindingModel());
            authorizationHandler.Verify(x => x.HandleRevokedPermissionBindingModelAsync(It.IsAny<IRevokedPermissionBindingModel>(), It.IsAny<ApiController>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostRemoveCollaboratorAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));
            var response = await controller.PostRemoveCollaboratorAsync(new ProgramCollaboratorBindingModel());
            authorizationHandler.Verify(x => x.HandleDeletedPermissionBindingModelAsync(It.IsAny<IDeletedPermissionBindingModel>(), It.IsAny<ApiController>()), Times.Once());
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
                ProgramStatusId = ProgramStatus.Active.Id,
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
