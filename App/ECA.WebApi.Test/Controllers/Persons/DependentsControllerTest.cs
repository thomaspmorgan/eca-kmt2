﻿using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Lookup;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Data;
using ECA.WebApi.Controllers.Persons;
using ECA.WebApi.Models.Person;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Persons
{
    [TestClass]
    public class DependentsControllerTest
    {
        private Mock<IPersonService> personService;
        private Mock<IUserProvider> userProvider;
        private Mock<IDependentTypeService> dependentTypeService;
        private Mock<IBirthCountryReasonService> birthCountryReasonService;
        private DependentsController controller;

        [TestInitialize]
        public void TestInit()
        {
            userProvider = new Mock<IUserProvider>();
            personService = new Mock<IPersonService>();
            dependentTypeService = new Mock<IDependentTypeService>();
            birthCountryReasonService = new Mock<IBirthCountryReasonService>();
            controller = new DependentsController(personService.Object, dependentTypeService.Object, birthCountryReasonService.Object, userProvider.Object);
        }

        [TestMethod]
        public async Task TestGetPersonDependentById()
        {
            personService.Setup(x => x.GetPersonDependentByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new SimplePersonDependentDTO());
            var response = await controller.GetPersonDependentByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<SimplePersonDependentDTO>));
        }

        [TestMethod]
        public async Task TestGetPersonDependentById_InvalidModel()
        {
            personService.Setup(x => x.GetPersonDependentByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null);
            var response = await controller.GetPersonDependentByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task TestPostDependentAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(0));
            personService.Setup(x => x.CreateDependentAsync(It.IsAny<NewPersonDependent>())).ReturnsAsync(new PersonDependent());
            personService.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var response = await controller.PostPersonDependentAsync(new DependentBindingModel { });
            personService.Verify(x => x.SaveChangesAsync(), Times.Once());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<SimplePersonDependentDTO>));
        }

        [TestMethod]
        public async Task TestPostPersonDependentAsync_Invalid()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostPersonDependentAsync(new DependentBindingModel());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public async Task TestGetDependentTypesAsync()
        {
            var response = await controller.GetDependentTypesAsync(new PagingQueryBindingModel<DependentTypeDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<DependentTypeDTO>>));
            dependentTypeService.Verify(x => x.GetAsync(It.IsAny<QueryableOperator<DependentTypeDTO>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetDependentTypesAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetDependentTypesAsync(new PagingQueryBindingModel<DependentTypeDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
            dependentTypeService.Verify(x => x.GetAsync(It.IsAny<QueryableOperator<DependentTypeDTO>>()), Times.Never());
        }
        
    }
}
