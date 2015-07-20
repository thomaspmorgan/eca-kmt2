using ECA.Business.Queries.Models.Persons;
using ECA.Business.Service.Persons;
using ECA.Core.DynamicLinq;
using ECA.Core.Query;
using ECA.Core.Service;
using ECA.Data;
using ECA.WebApi.Controllers.Persons;
using ECA.WebApi.Models.Admin;
using ECA.WebApi.Models.Person;
using ECA.WebApi.Models.Query;
using ECA.WebApi.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace ECA.WebApi.Test.Controllers.Persons
{
    [TestClass]
    public class PeopleControllerTest
    {
        private Mock<IPersonService> personService;
        private Mock<IUserProvider> userProvider;
        private Mock<IAddressModelHandler> addressHandler;
        private PeopleController controller;
        
        [TestInitialize]
        public void TestInit()
        {
            personService = new Mock<IPersonService>();
            userProvider = new Mock<IUserProvider>();
            addressHandler = new Mock<IAddressModelHandler>();
            controller = new PeopleController(personService.Object, userProvider.Object, addressHandler.Object);
        }

        #region Get Pii By Id
        [TestMethod]
        public async Task TestGetPiiById()
        {
            personService.Setup(x => x.GetPiiByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new PiiDTO());
            var response = await controller.GetPiiByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PiiDTO>));
        }

        [TestMethod]
        public async Task TestGetPiiById_InvalidModel()
        {
            personService.Setup(x => x.GetPiiByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null);
            var response = await controller.GetPiiByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }
        #endregion

        #region Get Contact Info By Id
        [TestMethod]
        public async Task TestGetContactInfoById()
        {
            personService.Setup(x => x.GetContactInfoByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new ContactInfoDTO());
            var response = await controller.GetContactInfoByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ContactInfoDTO>));
        }

        [TestMethod]
        public async Task TestGetContactInfoById_InvalidModel()
        {
            personService.Setup(x => x.GetContactInfoByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(null);
            var response = await controller.GetContactInfoByIdAsync(1);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }
        #endregion

        #region Post
        [TestMethod]
        public async Task TestPostPersonAsync()
        {
            userProvider.Setup(x => x.GetBusinessUser(It.IsAny<IWebApiUser>())).Returns(new Business.Service.User(0));
            personService.Setup(x => x.CreateAsync(It.IsAny<NewPerson>()))
                .ReturnsAsync(new Person());
            personService.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var response = await controller.PostPersonAsync(new PersonBindingModel());
            personService.Verify(x => x.SaveChangesAsync(), Times.Once());
            Assert.IsInstanceOfType(response, typeof(OkResult));
        }

        [TestMethod]
        public async Task TestPostPersonAsync_Invalid()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.PostPersonAsync(new PersonBindingModel());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        #endregion

        #region Get People
        [TestMethod]
        public async Task TestGetPeopleAsync()
        {
            var response = await controller.GetPeopleAsync(new PagingQueryBindingModel<SimplePersonDTO>());
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<PagedQueryResults<SimplePersonDTO>>));
            personService.Verify(x => x.GetPeopleAsync(It.IsAny<QueryableOperator<SimplePersonDTO>>()), Times.Once());
        }

        [TestMethod]
        public async Task TestGetPeopleAsync_InvalidModel()
        {
            controller.ModelState.AddModelError("key", "error");
            var response = await controller.GetPeopleAsync(new PagingQueryBindingModel<SimplePersonDTO>());
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }
        #endregion

        [TestMethod]
        public async Task TestPostAddressAsync()
        {
            var model = new PersonAddressBindingModel();
            var response = await controller.PostAddressAsync(model);
            addressHandler.Verify(x => x.HandleAdditionalAddress<Person>(It.IsAny<AddressBindingModelBase<Person>>(), It.IsAny<ApiController>()), Times.Once());
        }
    }
}
