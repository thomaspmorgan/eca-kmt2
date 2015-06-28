using CAM.Business.Model;
using CAM.Business.Queries.Models;
using CAM.Business.Service;
using CAM.Data;
using ECA.Business.Queries.Models.Admin;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Projects;
using ECA.Core.DynamicLinq;
using ECA.Core.DynamicLinq.Filter;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using ECA.WebApi.Controllers.Projects;
using ECA.WebApi.Models.Projects;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Models.Security;
using ECA.WebApi.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Projects
{
    [TestClass]
    public class ProjectsControllerTest
    {
        private Mock<IProjectService> service;
        private Mock<IUserProvider> userProvider;
        private Mock<IPrincipalService> principalService;
        private Mock<IResourceService> resourceService;
        
        private Mock<IResourceAuthorizationHandler> handler;
        private ProjectsController controller;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            service = new Mock<IProjectService>();
            principalService = new Mock<IPrincipalService>();
            resourceService = new Mock<IResourceService>();
            handler = new Mock<IResourceAuthorizationHandler>();

            service.Setup(x => x.GetProjectsByProgramIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<SimpleProjectDTO>>()))
                .ReturnsAsync(new PagedQueryResults<SimpleProjectDTO>(1, new List<SimpleProjectDTO>()));
            controller = new ProjectsController(service.Object, handler.Object, userProvider.Object, resourceService.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetProjectsByProgramIdAsync()
        {
            var response = await controller.GetProjectsByProgramAsync(1, new PagingQueryBindingModel<SimpleProjectDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimpleProjectDTO>>));
            service.Verify(x => x.GetProjectsByProgramIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<SimpleProjectDTO>>()), Times.Once());

        }

        [TestMethod]
        public async Task TestGetProjectsByProgramIdAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetProjectsByProgramAsync(1, new PagingQueryBindingModel<SimpleProjectDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Get Project By Id
        [TestMethod]
        public async Task TestGetProjectByIdAsync()
        {
            service.Setup(x => x.GetProjectByIdAsync(It.IsAny<int>()))
               .ReturnsAsync(new ProjectDTO());
            var response = await controller.GetProjectByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ProjectDTO>));
            service.Verify(x => x.GetProjectByIdAsync(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetProjectByIdAsync_InvalidModel()
        {
            service.Setup(x => x.GetProjectByIdAsync(It.IsAny<int>())).ReturnsAsync(null);
            var response = await controller.GetProjectByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }
        #endregion

        #region Get Projects By Person Id
        [TestMethod]
        public async Task TestGetProjectsByPersonIdAsync()
        {
            service.Setup(x => x.GetProjectsByPersonIdAsync(It.IsAny<int>(), It.IsAny<QueryableOperator<ParticipantTimelineDTO>>()))
                .ReturnsAsync(new PagedQueryResults<ParticipantTimelineDTO>(1, new List<ParticipantTimelineDTO>()));
            var response = await controller.GetProjectsByPersonIdAsync(1, new PagingQueryBindingModel<ParticipantTimelineDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<ParticipantTimelineDTO>>));
        }

        [TestMethod]
        public async Task TestGetProjectsByPersonIdAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetProjectsByPersonIdAsync(1, new PagingQueryBindingModel<ParticipantTimelineDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Post
        [TestMethod]
        public async Task TestPostProjectAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(0));
            service.Setup(x => x.CreateAsync(It.IsAny<DraftProject>()))
                .ReturnsAsync(new Project());
            service.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var response = await controller.PostProjectAsync(new DraftProjectBindingModel());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ProjectDTO>));
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestPostProjectAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostProjectAsync(new DraftProjectBindingModel());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Put
        [TestMethod]
        public async Task TestPutProjectAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(0));
            service.Setup(x => x.UpdateAsync(It.IsAny<PublishedProject>())).Returns(Task.FromResult<object>(null));
            service.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var response = await controller.PutProjectAsync(new PublishedProjectBindingModel());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ProjectDTO>));
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
        }

        [TestMethod]
        public async Task TestPutProjectAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PutProjectAsync(new PublishedProjectBindingModel());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Participants

        [TestMethod]
        public async Task TestPostAddPersonParticipantAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(0));
            service.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var model = new AdditionalPersonProjectParticipantBindingModel();
            model.ParticipantTypeId = ParticipantType.Individual.Id;
            var response = await controller.PostAddPersonParticipantAsync(model);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
            service.Verify(x => x.AddParticipantAsync(It.IsAny<AdditionalPersonProjectParticipant>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostAddPersonParticipantAsync_InvalidModel()
        {
            var model = new AdditionalPersonProjectParticipantBindingModel();
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostAddPersonParticipantAsync(model); 
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestPostAddOrganizationParticipantAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(0));
            service.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var model = new AdditionalOrganizationProjectPariticipantBindingModel();
            model.ParticipantTypeId = ParticipantType.Individual.Id;
            var response = await controller.PostAddOrganizationParticipantAsync(model);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            service.Verify(x => x.SaveChangesAsync(), Times.Once());
            service.Verify(x => x.AddParticipantAsync(It.IsAny<AdditionalOrganizationProjectParticipant>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostAddOrganizationParticipantAsync_InvalidModel()
        {
            var model = new AdditionalOrganizationProjectPariticipantBindingModel();
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostAddOrganizationParticipantAsync(model);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        #region Collaborators

        [TestMethod]
        public async Task TestGetCollaboratorDetailsAsync()
        {
            resourceService.Setup(x => x.GetResourceAuthorizationInfoDTOAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(new ResourceAuthorizationInfoDTO());
            var response = await controller.GetCollaboratorDetailsAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ResourceAuthorizationInfoDTO>));
            resourceService.Verify(x => x.GetResourceAuthorizationInfoDTOAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetCollaboratorDetailsAsync_DoesNotExist()
        {
            resourceService.Setup(x => x.GetResourceAuthorizationInfoDTOAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(null);
            var response = await controller.GetCollaboratorDetailsAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
            resourceService.Verify(x => x.GetResourceAuthorizationInfoDTOAsync(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostAddCollaboratorAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));

            var response = await controller.PostAddCollaboratorAsync(new CollaboratorBindingModel());
            handler.Verify(x => x.HandleGrantedPermissionBindingModelAsync(It.IsAny<IGrantedPermissionBindingModel>(), It.IsAny<ApiController>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostRevokeCollaboratorAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));
            var response = await controller.PostRevokeCollaboratorAsync(new CollaboratorBindingModel());
            handler.Verify(x => x.HandleRevokedPermissionBindingModelAsync(It.IsAny<IRevokedPermissionBindingModel>(), It.IsAny<ApiController>()), Times.Once());
        }

        [TestMethod]
        public async Task TestPostRemoveCollaboratorAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(1));
            var response = await controller.PostRemoveCollaboratorAsync(new CollaboratorBindingModel());
            handler.Verify(x => x.HandleDeletedPermissionBindingModelAsync(It.IsAny<IDeletedPermissionBindingModel>(), It.IsAny<ApiController>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetCollaboratorsAsync()
        {

            var projectId = 10;
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
                Assert.AreEqual(projectId, foreignResourceIdFilter.Value);

                var resourceTypeFilter = filters.Where(x => x.Property == "ResourceTypeId").FirstOrDefault();
                Assert.IsNotNull(resourceTypeFilter, "The resource type filter must exist.");
                Assert.AreEqual(ComparisonType.Equal.Value, resourceTypeFilter.Comparison);
                Assert.AreEqual(ResourceType.Project.Id, resourceTypeFilter.Value);
            };

            resourceService.Setup(x => x.GetResourceAuthorizationsAsync(It.IsAny<QueryableOperator<ResourceAuthorization>>()))
                .ReturnsAsync(new PagedQueryResults<ResourceAuthorization>(0, new List<ResourceAuthorization>()))
                .Callback(callbackTester);

            var response = await controller.GetCollaboratorsAsync(projectId, queryModel);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<ResourceAuthorization>>));
        }

        [TestMethod]
        public async Task TestGetCollaboratorsAsync_InvalidModelState()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetCollaboratorsAsync(1, new PagingQueryBindingModel<ResourceAuthorization>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        #endregion
    }
}
