﻿using ECA.Business.Queries.Models.Admin;
using ECA.Business.Queries.Models.Programs;
using ECA.Business.Service.Admin;
using ECA.Business.Service.Programs;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.WebApi.Controllers.Admin;
using ECA.WebApi.Controllers.Programs;
using ECA.WebApi.Models.Query;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Admin
{
    [TestClass]
    public class FocusControllerTest
    {
        private Mock<IFocusService> serviceMock;
        private FocusController controller;

        [TestInitialize]
        public void TestInit()
        {
            serviceMock = new Mock<IFocusService>();
            serviceMock.Setup(x => x.GetAsync(It.IsAny<QueryableOperator<FocusDTO>>()))
                .ReturnsAsync(new PagedQueryResults<FocusDTO>(1, new List<FocusDTO>()));
            controller = new FocusController(serviceMock.Object);
        }

        #region Get
        [TestMethod]
        public async Task TestGetThemesAsync()
        {
            var response = await controller.GetFociAsync(new PagingQueryBindingModel<FocusDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<FocusDTO>>));
        }

        [TestMethod]
        public async Task TestGetThemesAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetFociAsync(new PagingQueryBindingModel<FocusDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion
    }
}
