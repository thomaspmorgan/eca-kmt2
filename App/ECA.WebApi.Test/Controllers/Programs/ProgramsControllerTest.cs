using AutoMapper;
using ECA.Business.Models.Programs;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using ECA.WebApi.Controllers.Programs;
using ECA.WebApi.Models.Programs;
using ECA.WebApi.Models.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Programs
{
    [TestClass]
    public class ProgramsControllerTest
    {
        private Mock<IProgramService> serviceMock;
        private ProgramsController controller;

        [TestInitialize]
        public void TestInit()
        {
            Mapper.Reset();
            serviceMock = new Mock<IProgramService>();
            serviceMock.Setup(x => x.GetProgramsAsync(It.IsAny<QueryableOperator<SimpleProgramDTO>>()))
                .ReturnsAsync(new PagedQueryResults<SimpleProgramDTO>(1, new List<SimpleProgramDTO>()));
            serviceMock.Setup(x => x.CreateAsync(It.IsAny<DraftProgram>())).ReturnsAsync(new Program { RowVersion = new byte[0] });
            serviceMock.Setup(x => x.UpdateAsync(It.IsAny<EcaProgram>())).Returns(Task.FromResult<object>(null));
            serviceMock.Setup(x => x.SaveChangesAsync(It.IsAny<List<ISaveAction>>())).ReturnsAsync(1);
            serviceMock.Setup(x => x.GetProgramByIdAsync(It.IsAny<int>())).ReturnsAsync(new ProgramDTO { Id = 1, RowVersion = new byte[0] });
            controller = new ProgramsController(serviceMock.Object);
            ControllerHelper.InitializeController(controller);
        }

        #region Get
        [TestMethod]
        public async Task TestGetProgramsAsync()
        {
            var response = await controller.GetProgramsAsync(new PagingQueryBindingModel<SimpleProgramDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimpleProgramDTO>>));
        }

        [TestMethod]
        public async Task TestGetProgramsAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetProgramsAsync(new PagingQueryBindingModel<SimpleProgramDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestGetProgramByIdAsync()
        {
            serviceMock.Setup(x => x.GetProgramByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ProgramDTO { RowVersion = new byte[0] });
            var response = await controller.GetProgramByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ProgramViewModel>));
        }

        [TestMethod]
        public async Task TestGetProgramByIdAsync_ProgramDoesNotExist()
        {
            serviceMock.Setup(x => x.GetProgramByIdAsync(It.IsAny<int>())).ReturnsAsync(null);
            var response = await controller.GetProgramByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        #endregion

        #region Post
        [TestMethod]
        public async Task TestPostProgramAsync()
        {
            var model = new ProgramBindingModel
            {
                Name = "name",
                Description = "desc",
                ProgramStatusId = ProgramStatus.Active.Id
            };
            var response = await controller.PostProgramAsync(model);
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
            var model = new ProgramBindingModel
            {
                Name = "name",
                Description = "desc",
                ProgramStatusId = ProgramStatus.Active.Id,
                RowVersion = Convert.ToBase64String(new byte[0]),
            };
            model.ProgramStatusId = ProgramStatus.Active.Id;
            var response = await controller.PutProgramAsync(model);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ProgramViewModel>));
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
    }
}
